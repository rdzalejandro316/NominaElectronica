using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
//using NominaElectronica.ServiceNomina;
using SrvEnvio = WpfApp1.ServiceNomina;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SrvEnvio.ServiceClient serviceClienteEnvio = new SrvEnvio.ServiceClient();
        public string tokenEmpresa = "";
        public string tokenAuthorizacion = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvEnvio.Request request = new SrvEnvio.Request();                

                SrvEnvio.NominaGeneral nomina = new SrvEnvio.NominaGeneral();
                nomina.consecutivoDocumento = "FSNE1";

                #region deduccion

                
                SrvEnvio.Deduccion deduccion = new SrvEnvio.Deduccion();

                SrvEnvio.FondoPension fondo = new SrvEnvio.FondoPension()
                {
                    deduccion = "10000.00",
                    porcentaje = "10.00"
                };
                deduccion.fondosPensiones = new SrvEnvio.FondoPension[1];
                deduccion.fondosPensiones[0] = fondo;                                

                SrvEnvio.Salud salud = new SrvEnvio.Salud
                {
                    deduccion = "10000.00",
                    porcentaje = "10.00"
                };
                deduccion.salud = new SrvEnvio.Salud[1];
                deduccion.salud[0] = salud;

                nomina.deducciones = deduccion;

                #endregion

                #region devengado

                
                SrvEnvio.Devengado devengado = new SrvEnvio.Devengado();

                SrvEnvio.Basico basico = new SrvEnvio.Basico()
                {
                    diasTrabajados = "15",
                    sueldoTrabajado = "500000.00"
                };
                devengado.basico = new SrvEnvio.Basico[1];
                devengado.basico[0] = basico;

                nomina.devengados = devengado;
                #endregion


                nomina.fechaEmision = "2021-03-02 12:00:00";

                nomina.lugarGeneracionXML = new SrvEnvio.LugarGeneracionXML()
                {
                    departamentoEstado = "11",
                    idioma = "es",
                    municipioCiudad = "11001",
                    pais = "CO"
                };


                SrvEnvio.FechaPago fechaPago = new SrvEnvio.FechaPago();
                fechaPago.fechapagonomina = "2021-03-15";


                SrvEnvio.Pago pago = new SrvEnvio.Pago();
                pago.fechasPagos = new SrvEnvio.FechaPago[1];
                pago.fechasPagos[0] = fechaPago;
                pago.medioPago = "1";
                pago.nombreBanco = "Nombre del Banco";
                pago.numeroCuenta = "123456789";
                pago.tipoCuenta = "Ahorro";

                nomina.pagos = new SrvEnvio.Pago[1];
                nomina.pagos[0] = pago;

                nomina.periodoNomina = "4";

                nomina.rangoNumeracion = "FSNE-1";
                nomina.redondeo = "0.00";
                nomina.tipoDocumento = "102";
                nomina.tipoMoneda = "COP";
                nomina.totalComprobante = "410000.00";
                nomina.totalDeducciones = "40000.00";
                nomina.totalDevengados = "500000.00";


                nomina.trabajador = new SrvEnvio.Trabajador()
                {
                    altoRiesgoPension = "0",
                    codigoTrabajador = "A527",
                    email = "jorge.alejandro@siasoftsas.com",
                    lugarTrabajoDepartamentoEstado = "11",
                    lugarTrabajoDireccion = "crr 5 # 49 d 230 sur",
                    lugarTrabajoMunicipioCiudad = "11001",
                    lugarTrabajoPais = "CO",
                    numeroDocumento = "1033796537",
                    primerApellido = "garcia",
                    primerNombre = "Anderson",
                    salarioIntegral = "0",
                    segundoApellido = "rojas",
                    subTipoTrabajador = "01",
                    sueldo = "500000.00",
                    tipoContrato = "1",
                    tipoIdentificacion = "31",
                    tipoTrabajador = "01"
                };

                nomina.trm = "3000.00";

                request.idSoftware = "123456";
                request.nitEmpleador = "1033796537";
                request.tokenEnterprise = "5288b0621f424a49b6c4f9750a3fd1e5b4884da9";
                request.tokenPassword = "de0e20662a20462ba64bcbdf6405e6e9ac842849";
                request.nomina = nomina;

                string filename = "nomina.xml";
                string ArchivoRequest = @"C:\Users\aleja\Desktop\" + filename;
                StreamWriter MyFile = new StreamWriter(ArchivoRequest);
                XmlSerializer Serializer1 = new XmlSerializer(typeof(SrvEnvio.Request));

                Serializer1.Serialize(MyFile, request);
                MyFile.Close();



                rtxInformacion.Text += "** asincoron ** ";
                Task<SrvEnvio.Response> docRespuesta;
                docRespuesta = serviceClienteEnvio.EnviarAsync(request);

                if (docRespuesta.IsCompleted)
                {
                    //StringBuilder msgError = new StringBuilder();
                    //if (docRespuesta.Result. != null)
                    //{
                    //    int nReturnMsg = docRespuesta.Result.mensajesValidacion.Count();
                    //    for (int i = 0; i < nReturnMsg; i++)
                    //        msgError.Append(docRespuesta.Result.mensajesValidacion[i].ToString() + Environment.NewLine);
                    //}


                    rtxInformacion.Text += "** INICIA ** ";
                    if (docRespuesta.Result.codigo == "200")
                    {
                        StringBuilder response = new StringBuilder();
                        response.Append("Codigo: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                        response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                        response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                        response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                        response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);
                        rtxInformacion.Text += response.ToString();
                    }

                    rtxInformacion.Text += "** ENVIA ** ";
                    //else
                    //{
                    //    StringBuilder response = new StringBuilder();
                    //    response.Append("xxx Codigo xxx:" + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                    //    response.Append("Consecutivo Documento :" + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                    //    response.Append("Mensaje :" + docRespuesta.Result.mensaje + Environment.NewLine);
                    //    response.Append("Resultado :" + docRespuesta.Result.resultado + Environment.NewLine);
                    //    response.Append("Errores :" + msgError + Environment.NewLine);
                    //    rtxInformacion.Text += response.ToString();

                    //}

                }
                else
                {
                    rtxInformacion.Text += "** FALLo ** ";
                    StringBuilder response = new StringBuilder();
                    response.Append("Codigo: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                    response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                    response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                    response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                    response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);
                    rtxInformacion.Text += response.ToString();
                }


            }
            catch (Exception w)
            {
                MessageBox.Show("error al enviar:" + w);
            }
        }
    }
}
