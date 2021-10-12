using Microsoft.Win32;
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
using SrvEnvio = WpfApp1.ServiceNomina;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SrvEnvio.ServiceClient serviceClienteEnvio = new SrvEnvio.ServiceClient();
        public string tokenEmpresa = "5288b0621f424a49b6c4f9750a3fd1e5b4884da9";
        public string tokenAuthorizacion = "de0e20662a20462ba64bcbdf6405e6e9ac842849";
        public string NitAutorizado = "832005853";


        string filename = "nomina.xml";
        string ArchivoRequest = "";

        public MainWindow()
        {
            InitializeComponent();
            ArchivoRequest = $"{AppDomain.CurrentDomain.BaseDirectory}{filename}";
            TxPath.Text = ArchivoRequest;

        }

        private async void BtnEnviarMinimos_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SrvEnvio.Request request = new SrvEnvio.Request();


                #region nomina general

                SrvEnvio.NominaGeneral nomina = new SrvEnvio.NominaGeneral();
                nomina.consecutivoDocumentoNom = "NOM10021";
                nomina.fechaEmisionNom = "2021-03-02 12:00:00";
                nomina.periodoNomina = "4";
                nomina.rangoNumeracionNom = "NOM-1";
                nomina.redondeo = "0.00";
                nomina.tipoDocumentoNom = "102";
                nomina.tipoMonedaNom = "COP";
                nomina.trm = "3000.00";
                nomina.totalComprobante = "410000.00";
                nomina.totalDeducciones = "40000.00";
                nomina.totalDevengados = "500000.00";

                #endregion               

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
                    diasTrabajados = "30",
                    sueldoTrabajado = "500000.00"
                };
                devengado.basico = new SrvEnvio.Basico[1];
                devengado.basico[0] = basico;

                nomina.devengados = devengado;
                #endregion

                #region lugar de emision 

                nomina.lugarGeneracionXML = new SrvEnvio.LugarGeneracionXML()
                {
                    departamentoEstado = "11",
                    idioma = "es",
                    municipioCiudad = "11001",
                    pais = "CO"
                };
                #endregion

                #region pago


                SrvEnvio.FechaPago fechaPago = new SrvEnvio.FechaPago();
                fechaPago.fechapagonomina = "2021-03-15";


                SrvEnvio.Pago pago = new SrvEnvio.Pago();
                pago.fechasPagos = new SrvEnvio.FechaPago[1];
                pago.fechasPagos[0] = fechaPago;
                pago.medioPago = "ZZZ";
                pago.metodoDePago = "1";
                pago.nombreBanco = "Nombre del Banco";
                pago.numeroCuenta = "123456789";
                pago.tipoCuenta = "Ahorro";

                nomina.pagos = new SrvEnvio.Pago[1];
                nomina.pagos[0] = pago;


                SrvEnvio.Periodo periodo = new SrvEnvio.Periodo();
                periodo.fechaIngreso = "2021-03-02";
                periodo.fechaLiquidacionFin = "2021-03-04";
                periodo.fechaLiquidacionInicio = "2021-03-03";
                periodo.tiempoLaborado = "3";

                nomina.periodos = new SrvEnvio.Periodo[1];
                nomina.periodos[0] = periodo;

                #endregion

                #region datos trabajador               
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
                #endregion


                request.idSoftware = "123456";
                request.nitEmpleador = NitAutorizado;
                request.tokenEnterprise = tokenEmpresa;
                request.tokenPassword = tokenAuthorizacion;

                request.nomina = nomina;


                StreamWriter MyFile = new StreamWriter(ArchivoRequest);
                XmlSerializer Serializer1 = new XmlSerializer(typeof(SrvEnvio.Request));

                Serializer1.Serialize(MyFile, request);
                MyFile.Close();



                rtxInformacion.Text += "** asincoron ** ";
                Task<SrvEnvio.Response> docRespuesta;
                docRespuesta = serviceClienteEnvio.EnviarAsync(request);
                await docRespuesta;

                if (docRespuesta.IsCompleted)
                {

                    rtxInformacion.Text += "** INICIA ** ";
                    if (docRespuesta.Result.codigo == "200")
                    {
                        StringBuilder response = new StringBuilder();
                        response.Append("Codigo **ES 200***: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                        response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                        response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                        response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                        response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);

                        if (docRespuesta.Result.reglasRechazoTFHKA != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                            }
                        }

                        if (docRespuesta.Result.reglasRechazoDIAN != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                            }
                        }


                        rtxInformacion.Text += response.ToString();
                    }
                    else
                    {
                        StringBuilder response = new StringBuilder();
                        response.Append("ERROR EN EL ENVIO" + Environment.NewLine);
                        response.Append("Codigo **NO ES 200***: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                        response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                        response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                        response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                        response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);


                        if (docRespuesta.Result.reglasRechazoTFHKA != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                            }
                        }

                        if (docRespuesta.Result.reglasRechazoDIAN != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                            }
                        }


                        rtxInformacion.Text += response.ToString();

                    }

                    rtxInformacion.Text += "** ENVIA ** ";

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


                    if (docRespuesta.Result.reglasRechazoTFHKA != null)
                    {
                        int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                        for (int i = 0; i < nReturnMsg; i++)
                        {
                            response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                        }
                    }

                    if (docRespuesta.Result.reglasRechazoDIAN != null)
                    {
                        int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                        for (int i = 0; i < nReturnMsg; i++)
                        {
                            response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                        }
                    }


                    rtxInformacion.Text += response.ToString();
                }


            }
            catch (Exception w)
            {
                MessageBox.Show("error al enviar:" + w);
            }
        }

        private async void BtnEnviarCompletos_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SrvEnvio.Request request = new SrvEnvio.Request();


                #region nomina general

                SrvEnvio.NominaGeneral nomina = new SrvEnvio.NominaGeneral();
                nomina.consecutivoDocumentoNom = "NOM10021";
                nomina.fechaEmisionNom = "2021-03-02 12:00:00";
                nomina.periodoNomina = "4";
                nomina.rangoNumeracionNom = "NOM-1";
                nomina.redondeo = "0.00";
                nomina.tipoDocumentoNom = "102";
                nomina.tipoMonedaNom = "COP";
                nomina.trm = "3000.00";
                nomina.totalComprobante = "410000.00";
                nomina.totalDeducciones = "40000.00";
                nomina.totalDevengados = "500000.00";

                #endregion              

                #region deduccion


                SrvEnvio.Deduccion deduccion = new SrvEnvio.Deduccion();

                #region AFC (Ahorro Fomento a la construcción)
                deduccion.afc = "10000.00";
                #endregion

                #region coopertativa                
                deduccion.cooperativa = "10000.00";
                #endregion

                #region deuda                
                deduccion.deuda = "10000.00";
                #endregion

                #region educacion                
                deduccion.educacion = "10000.00";
                #endregion

                #region embargo fical
                deduccion.embargoFiscal = "10000.00";
                #endregion

                #region pension voluntaria
                deduccion.pensionVoluntaria = "10000.00";
                #endregion

                #region plan complementario
                deduccion.planComplementarios = "10000.00";
                #endregion

                #region reintegro
                deduccion.reintegro = "10000.00";
                #endregion

                #region retencion en la fuente
                deduccion.retencionFuente = "10000.00";
                #endregion

                #region anticipo


                SrvEnvio.AnticipoNom anticipo_deduccion = new SrvEnvio.AnticipoNom()
                {
                    montoanticipo = "10000.00"
                };
                deduccion.anticiposNom = new SrvEnvio.AnticipoNom[1];
                deduccion.anticiposNom[0] = anticipo_deduccion;
                #endregion

                #region fondo de pensiones                

                SrvEnvio.FondoPension fondo = new SrvEnvio.FondoPension()
                {
                    deduccion = "10000.00",
                    porcentaje = "10.00"
                };
                deduccion.fondosPensiones = new SrvEnvio.FondoPension[1];
                deduccion.fondosPensiones[0] = fondo;

                #endregion

                #region fondo de seguridad pensional

                SrvEnvio.FondoSP fondoSP = new SrvEnvio.FondoSP
                {
                    deduccionSP = "10000.00",
                    deduccionSub = "10000.00",
                    porcentaje = "10.00",
                    porcentajeSub = "10.00",
                };
                deduccion.fondosSP = new SrvEnvio.FondoSP[1];
                deduccion.fondosSP[0] = fondoSP;

                #endregion

                #region libranzas

                SrvEnvio.Libranza libranza = new SrvEnvio.Libranza
                {
                    deduccion = "10000.00",
                    descripcion = "Descripcion General de Libranza"
                };
                deduccion.libranzas = new SrvEnvio.Libranza[1];
                deduccion.libranzas[0] = libranza;
                #endregion

                #region otras deducciones

                SrvEnvio.OtraDeduccion otraDeduccion = new SrvEnvio.OtraDeduccion
                {
                    montootraDeduccion = "10000.00"
                };
                deduccion.otrasDeducciones = new SrvEnvio.OtraDeduccion[1];
                deduccion.otrasDeducciones[0] = otraDeduccion;
                #endregion

                #region pago de terceros

                SrvEnvio.PagoTercero pagoTercero = new SrvEnvio.PagoTercero
                {
                    montopagotercero = "10000.00"
                };
                deduccion.pagosTerceros = new SrvEnvio.PagoTercero[1];
                deduccion.pagosTerceros[0] = pagoTercero;
                #endregion

                #region salud

                SrvEnvio.Salud salud = new SrvEnvio.Salud
                {
                    deduccion = "10000.00",
                    porcentaje = "10.00"
                };
                deduccion.salud = new SrvEnvio.Salud[1];
                deduccion.salud[0] = salud;
                #endregion

                #region sanciones

                SrvEnvio.Sancion sancion = new SrvEnvio.Sancion
                {
                    sancionPriv = "10000.00",
                    sancionPublic = "10000.00"
                };
                deduccion.sanciones = new SrvEnvio.Sancion[1];
                deduccion.sanciones[0] = sancion;
                #endregion

                #region sindicatos

                SrvEnvio.Sindicato sindicato = new SrvEnvio.Sindicato
                {
                    deduccion = "10000.00",
                    porcentaje = "10.00"
                };
                deduccion.sindicatos = new SrvEnvio.Sindicato[1];
                deduccion.sindicatos[0] = sindicato;
                #endregion


                nomina.deducciones = deduccion;

                #endregion

                #region devengado

                SrvEnvio.Devengado devengado = new SrvEnvio.Devengado();

                #region anticipo


                SrvEnvio.AnticipoNom anticipo_devengado = new SrvEnvio.AnticipoNom()
                {
                    montoanticipo = "100000.00"
                };
                devengado.anticiposNom = new SrvEnvio.AnticipoNom[1];
                devengado.anticiposNom[0] = anticipo_devengado;

                #endregion

                #region apoyo aprensiz o practicante
                devengado.apoyoSost = "10000.00";
                #endregion

                #region auxilio

                SrvEnvio.Auxilio auxilio = new SrvEnvio.Auxilio()
                {
                    auxilioNS = "10000.00",
                    auxilioS = "10000.00"
                };
                devengado.auxilios = new SrvEnvio.Auxilio[1];
                devengado.auxilios[0] = auxilio;

                #endregion

                #region basico

                SrvEnvio.Basico basico = new SrvEnvio.Basico()
                {
                    diasTrabajados = "30",
                    sueldoTrabajado = "500000.00"
                };
                devengado.basico = new SrvEnvio.Basico[1];
                devengado.basico[0] = basico;

                #endregion

                #region bonificaciones

                SrvEnvio.Bonificacion bonificacion = new SrvEnvio.Bonificacion()
                {
                    bonificacionNS = "10000.00",
                    bonificacionS = "10000.00"
                };
                devengado.bonificaciones = new SrvEnvio.Bonificacion[1];
                devengado.bonificaciones[0] = bonificacion;

                #endregion

                #region bonificacion (Valor establecido por mutuo acuerdo por retiro del Trabajador)

                devengado.bonifRetiro = "10000.00";
                #endregion

                #region bonificaciones EPCTV (Todos los Elementos de Bonos Electrónicos o de Papel de Servicio, Cheques, Tarjetas, Vales, etc )

                SrvEnvio.BonoEPCTV bonoEPCTV = new SrvEnvio.BonoEPCTV()
                {
                    pagoAlimentacionNS = "10000.00",
                    pagoAlimentacionS = "10000.00",
                    pagoNS = "10000.00",
                    pagoS = "10000.00",

                };
                devengado.bonoEPCTVs = new SrvEnvio.BonoEPCTV[1];
                devengado.bonoEPCTVs[0] = bonoEPCTV;

                #endregion

                #region cesantias

                SrvEnvio.Cesantia cesantia = new SrvEnvio.Cesantia()
                {
                    pago = "10000.00",
                    pagoIntereses = "10000.00",
                    porcentaje = "10.00",

                };
                devengado.cesantias = new SrvEnvio.Cesantia[1];
                devengado.cesantias[0] = cesantia;

                #endregion

                #region comision

                SrvEnvio.Comision comision = new SrvEnvio.Comision()
                {
                    montocomision = "10000.00"
                };
                devengado.comisiones = new SrvEnvio.Comision[1];
                devengado.comisiones[0] = comision;

                #endregion

                #region compensaciones

                SrvEnvio.Compensacion compensacion = new SrvEnvio.Compensacion()
                {
                    compensacionE = "10000.00",
                    compensacionO = "10000.00"
                };
                devengado.compensaciones = new SrvEnvio.Compensacion[1];
                devengado.compensaciones[0] = compensacion;

                #endregion

                #region horas extra

                SrvEnvio.HoraExtra horas = new SrvEnvio.HoraExtra()
                {
                    cantidad = "10000.00",
                    horaFin = "2021-03-02 12:00:00",
                    horaInicio = "2021-03-02 12:00:00",
                    pago = "10000.00",
                    porcentaje = "25.00",
                    tipoHorasExtra = "0",
                };
                devengado.horasExtras = new SrvEnvio.HoraExtra[1];
                devengado.horasExtras[0] = horas;

                #endregion

                #region huelgas

                SrvEnvio.HuelgaLegal huelga = new SrvEnvio.HuelgaLegal()
                {
                    cantidad = "30",
                    fechaInicio = "2021-03-05",
                    fechaFin = "2021-03-15"
                };
                devengado.huelgasLegales = new SrvEnvio.HuelgaLegal[1];
                devengado.huelgasLegales[0] = huelga;

                #endregion

                #region incapacidad

                SrvEnvio.Incapacidad incapacidad = new SrvEnvio.Incapacidad()
                {
                    cantidad = "30",
                    fechaInicio = "2021-03-05",
                    fechaFin = "2021-03-15",
                    pago = "10000.00",
                    tipo = "2"
                };
                devengado.incapacidades = new SrvEnvio.Incapacidad[1];
                devengado.incapacidades[0] = incapacidad;

                #endregion

                #region licencias

                devengado.licencias = new SrvEnvio.Licencias();

                #region licencia MP
                SrvEnvio.Licencia licenciaMP = new SrvEnvio.Licencia()
                {
                    cantidad = "30",
                    fechaInicio = "2021-03-05",
                    fechaFin = "2021-03-15",
                    pago = "10000.00"
                };
                devengado.licencias.licenciaMP = new SrvEnvio.Licencia[1];
                devengado.licencias.licenciaMP[0] = licenciaMP;
                #endregion

                #region licencia NR                
                SrvEnvio.Licencia licenciaNR = new SrvEnvio.Licencia()
                {
                    cantidad = "30",
                    fechaInicio = "2021-03-05",
                    fechaFin = "2021-03-15",
                    pago = "10000.00"
                };
                devengado.licencias.licenciaNR = new SrvEnvio.Licencia[1];
                devengado.licencias.licenciaNR[0] = licenciaNR;
                #endregion

                #region licencia R                
                SrvEnvio.Licencia licenciaR = new SrvEnvio.Licencia()
                {
                    cantidad = "30",
                    fechaInicio = "2021-03-05",
                    fechaFin = "2021-03-15",
                    pago = "10000.00"
                };
                devengado.licencias.licenciaR = new SrvEnvio.Licencia[1];
                devengado.licencias.licenciaR[0] = licenciaR;
                #endregion

                #endregion

                #region otros conceptos

                SrvEnvio.OtroConcepto otroConcepto = new SrvEnvio.OtroConcepto()
                {
                    conceptoNS = "10000.00",
                    conceptoS = "10000.00",
                    descripcionConcepto = "Descipcion Generica",
                };

                devengado.otrosConceptos = new SrvEnvio.OtroConcepto[1];
                devengado.otrosConceptos[0] = otroConcepto;

                #endregion

                #region pago a terceros

                SrvEnvio.PagoTercero pagoTercero_devengado = new SrvEnvio.PagoTercero()
                {
                    montopagotercero = "10000.00"
                };

                devengado.pagosTerceros = new SrvEnvio.PagoTercero[1];
                devengado.pagosTerceros[0] = pagoTercero_devengado;

                #endregion

                #region prima

                SrvEnvio.Prima prima = new SrvEnvio.Prima()
                {
                    cantidad = "30",
                    pago = "10000.00",
                    pagoNS = "10000.00"
                };

                devengado.primas = new SrvEnvio.Prima[1];
                devengado.primas[0] = prima;

                #endregion

                #region reintegro                
                devengado.reintegro = "10000.00";
                #endregion

                #region tele trabajo            
                devengado.teletrabajo = "10000.00";
                #endregion

                #region transporte

                SrvEnvio.Transporte transporte = new SrvEnvio.Transporte()
                {
                    auxilioTransporte = "10000.00",
                    viaticoManuAlojNS = "10000.00",
                    viaticoManuAlojS = "10000.00"
                };

                devengado.primas = new SrvEnvio.Prima[1];
                devengado.primas[0] = prima;

                #endregion

                #region vacaciones

                devengado.vacaciones = new SrvEnvio.Vacacion();

                #region vacaciones compensadas

                SrvEnvio.Vacaciones vacaciones_compensadas = new SrvEnvio.Vacaciones()
                {
                    cantidad = "30",
                    fechaInicio = "2021-03-15",
                    fechaFin = "2021-03-15",
                    pago = "10000.00",
                };
                devengado.vacaciones.vacacionesCompensadas = new SrvEnvio.Vacaciones[1];
                devengado.vacaciones.vacacionesCompensadas[0] = vacaciones_compensadas;
                #endregion

                #region vacaciones comunes

                SrvEnvio.Vacaciones vacaciones_comunes = new SrvEnvio.Vacaciones()
                {
                    cantidad = "30",
                    fechaInicio = "2021-03-15",
                    fechaFin = "2021-03-15",
                    pago = "10000.00",
                };
                devengado.vacaciones.vacacionesComunes = new SrvEnvio.Vacaciones[1];
                devengado.vacaciones.vacacionesComunes[0] = vacaciones_comunes;

                #endregion

                #endregion

                nomina.devengados = devengado;
                #endregion

                #region documento referenciado (corresponder al CUNE del Documento Soporte de Pago de Nómina Electrónica o Nota de Ajuste de Documento Soporte de Pago de Nómina Electrónica a Reemplazar)


                SrvEnvio.DocumentoReferenciadoNom documentoReferenciado = new SrvEnvio.DocumentoReferenciadoNom()
                {
                    cunePred = "d91deada982e658a3f69bf8eb2d173c7c10142fe34dee71f9c3c75b3d066b2f535083f02657a79369efdd6d9dc11240e",
                    fechaGenPred = "2021-03-04",
                    numeroPred = "DSNE1"
                };

                nomina.documentosReferenciadosNom = new SrvEnvio.DocumentoReferenciadoNom[1];
                nomina.documentosReferenciadosNom[0] = documentoReferenciado;


                #endregion

                #region lugar de emision 

                nomina.lugarGeneracionXML = new SrvEnvio.LugarGeneracionXML()
                {
                    departamentoEstado = "11",
                    idioma = "es",
                    municipioCiudad = "11001",
                    pais = "CO"
                };
                #endregion

                #region nota

                SrvEnvio.Nota nota = new SrvEnvio.Nota()
                {
                    descripcion = "nota de prueba 1"
                };

                nomina.notas = new SrvEnvio.Nota[1];
                nomina.notas[0] = nota;


                #endregion

                #region novedad                
                nomina.novedad = "0";
                nomina.novedadCUNE = "d91deada982e658a3f69bf8eb2d173c7c10142fe34dee71f9c3c75b3d066b2f535083f02657a79369efdd6d9dc11240e";
                #endregion

                #region pagos


                SrvEnvio.FechaPago fechaPago = new SrvEnvio.FechaPago();
                fechaPago.fechapagonomina = "2021-03-15";


                SrvEnvio.Pago pago = new SrvEnvio.Pago();
                pago.fechasPagos = new SrvEnvio.FechaPago[1];
                pago.fechasPagos[0] = fechaPago;
                pago.medioPago = "ZZZ";
                pago.metodoDePago = "1";
                pago.nombreBanco = "Nombre del Banco";
                pago.numeroCuenta = "123456789";
                pago.tipoCuenta = "Ahorro";

                nomina.pagos = new SrvEnvio.Pago[1];
                nomina.pagos[0] = pago;


                SrvEnvio.Periodo periodo = new SrvEnvio.Periodo();
                periodo.fechaIngreso = "2021-03-02";
                periodo.fechaLiquidacionFin = "2021-03-04";
                periodo.fechaLiquidacionInicio = "2021-03-03";
                periodo.tiempoLaborado = "3";

                nomina.periodos = new SrvEnvio.Periodo[1];
                nomina.periodos[0] = periodo;

                #endregion

                #region datos trabajador               
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
                #endregion


                request.idSoftware = "123456";
                request.nitEmpleador = NitAutorizado;
                request.tokenEnterprise = tokenEmpresa;
                request.tokenPassword = tokenAuthorizacion;


                request.nomina = nomina;


                StreamWriter MyFile = new StreamWriter(ArchivoRequest);
                XmlSerializer Serializer1 = new XmlSerializer(typeof(SrvEnvio.Request));

                Serializer1.Serialize(MyFile, request);
                MyFile.Close();



                rtxInformacion.Text += "** asincoron ** ";
                Task<SrvEnvio.Response> docRespuesta;
                docRespuesta = serviceClienteEnvio.EnviarAsync(request);
                await docRespuesta;

                if (docRespuesta.IsCompleted)
                {

                    rtxInformacion.Text += "** INICIA ** ";
                    if (docRespuesta.Result.codigo == "200")
                    {
                        StringBuilder response = new StringBuilder();
                        response.Append("Codigo 200**&&&&***: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                        response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                        response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                        response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                        response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);
                        if (docRespuesta.Result.reglasRechazoTFHKA != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                            }
                        }

                        if (docRespuesta.Result.reglasRechazoDIAN != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                            }
                        }

                        rtxInformacion.Text += response.ToString();
                    }
                    else
                    {
                        StringBuilder response = new StringBuilder();
                        response.Append("ERROR EN EL ENVIO" + Environment.NewLine);
                        response.Append("Codigo #######" + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                        response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                        response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                        response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                        response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);
                        if (docRespuesta.Result.reglasRechazoTFHKA != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                            }
                        }

                        if (docRespuesta.Result.reglasRechazoDIAN != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                            }
                        }
                        rtxInformacion.Text += response.ToString();

                    }

                    rtxInformacion.Text += "** ENVIA ** ";

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
                    if (docRespuesta.Result.reglasRechazoTFHKA != null)
                    {
                        int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                        for (int i = 0; i < nReturnMsg; i++)
                        {
                            response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                        }
                    }

                    if (docRespuesta.Result.reglasRechazoDIAN != null)
                    {
                        int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                        for (int i = 0; i < nReturnMsg; i++)
                        {
                            response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                        }
                    }
                    rtxInformacion.Text += response.ToString();
                }


            }
            catch (Exception w)
            {
                MessageBox.Show("error al enviar:" + w);
            }
        }

        private async void BtnEstado_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvEnvio.RequestConsultarDocumento requestConsultarDocumento = new SrvEnvio.RequestConsultarDocumento();
                requestConsultarDocumento.consecutivoDocumentoNom = TxDoc.Text;
                requestConsultarDocumento.tokenEnterprise = tokenEmpresa;
                requestConsultarDocumento.tokenPassword = tokenAuthorizacion;

                var request = await serviceClienteEnvio.EstadoDocumentoAsync(requestConsultarDocumento);

                rtxInformacion.Text += request.cadenaCodigoQR;
                rtxInformacion.Text += request.codigo;
                rtxInformacion.Text += request.consecutivo;
                rtxInformacion.Text += request.descripcionDocumento;
                rtxInformacion.Text += request.resultado;

            }
            catch (Exception w)
            {
                MessageBox.Show("error en ver el estado del documento:" + w);
            }
        }

        private async void BtnXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvEnvio.RequestConsultarDocumento requestConsultarDocumento = new SrvEnvio.RequestConsultarDocumento();
                requestConsultarDocumento.consecutivoDocumentoNom = TxDoc.Text;
                requestConsultarDocumento.tokenEnterprise = tokenEmpresa;
                requestConsultarDocumento.tokenPassword = tokenAuthorizacion;
                SrvEnvio.ResponseDownloadDocument request = await serviceClienteEnvio.DescargaXMLAsync(requestConsultarDocumento);
                
                if (request.codigo == 200)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "XML|*.xml";
                    saveFileDialog.Title = "Save File";
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        string path = saveFileDialog.FileName;
                        File.WriteAllBytes(path, Convert.FromBase64String(request.documento));
                        MessageBox.Show("se guardo el archivo exitosamente", "alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }


            }
            catch (Exception w)
            {
                MessageBox.Show("error al descargar el XML:" + w);
            }
        }

        private async void BtnPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvEnvio.RequestConsultarDocumento requestConsultarDocumento = new SrvEnvio.RequestConsultarDocumento();
                requestConsultarDocumento.consecutivoDocumentoNom = TxDoc.Text;
                requestConsultarDocumento.tokenEnterprise = tokenEmpresa;
                requestConsultarDocumento.tokenPassword = tokenAuthorizacion;
                SrvEnvio.ResponseDownloadDocument request = await serviceClienteEnvio.DescargaPDFAsync(requestConsultarDocumento);

                if (request.codigo == 200)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Pdf|*.pdf";
                    saveFileDialog.Title = "Save File";
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        string path = saveFileDialog.FileName;
                        File.WriteAllBytes(path, Convert.FromBase64String(request.documento));
                        MessageBox.Show("se guardo el archivo exitosamente", "alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                


            }
            catch (Exception w)
            {
                MessageBox.Show("error al descargar el XML:" + w);
            }
        }

        private async void BtnEnviarAjusteMinimos_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SrvEnvio.Request request = new SrvEnvio.Request();


                #region nomina general

                SrvEnvio.NominaGeneral nomina = new SrvEnvio.NominaGeneral();
                nomina.consecutivoDocumentoNom = "AJ112";
                nomina.fechaEmisionNom = "2021-03-02 12:00:00";
                nomina.periodoNomina = "4";
                nomina.rangoNumeracionNom = "AJ-1";
                nomina.redondeo = "0.00";
                nomina.tipoDocumentoNom = "103";
                nomina.tipoNota = "1";
                nomina.tipoMonedaNom = "COP";
                nomina.trm = "3000.00";
                nomina.totalComprobante = "410000.00";
                nomina.totalDeducciones = "40000.00";
                nomina.totalDevengados = "500000.00";

                #endregion               

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


                #region ajuste documento
                SrvEnvio.DocumentoReferenciadoNom referenciadosNom = new SrvEnvio.DocumentoReferenciadoNom();
                referenciadosNom.cunePred = "7b37fe921b2c14452ea6553ee70db5f60bbdfdf0135418a1fc51de98158e61660252b81abaaf4f9bdffad09b7c5ec555";
                referenciadosNom.fechaGenPred = "2021-03-04";
                referenciadosNom.numeroPred = "NOM10012";

                nomina.documentosReferenciadosNom = new SrvEnvio.DocumentoReferenciadoNom[1];
                nomina.documentosReferenciadosNom[0] = referenciadosNom;


                #endregion

                #region lugar de emision 

                nomina.lugarGeneracionXML = new SrvEnvio.LugarGeneracionXML()
                {
                    departamentoEstado = "11",
                    idioma = "es",
                    municipioCiudad = "11001",
                    pais = "CO"
                };
                #endregion

                #region pago


                SrvEnvio.FechaPago fechaPago = new SrvEnvio.FechaPago();
                fechaPago.fechapagonomina = "2021-03-15";


                SrvEnvio.Pago pago = new SrvEnvio.Pago();
                pago.fechasPagos = new SrvEnvio.FechaPago[1];
                pago.fechasPagos[0] = fechaPago;
                pago.medioPago = "ZZZ";
                pago.metodoDePago = "1";
                pago.nombreBanco = "Nombre del Banco";
                pago.numeroCuenta = "123456789";
                pago.tipoCuenta = "Ahorro";

                nomina.pagos = new SrvEnvio.Pago[1];
                nomina.pagos[0] = pago;


                SrvEnvio.Periodo periodo = new SrvEnvio.Periodo();
                periodo.fechaIngreso = "2021-03-02";
                periodo.fechaLiquidacionFin = "2021-03-04";
                periodo.fechaLiquidacionInicio = "2021-03-03";
                periodo.tiempoLaborado = "3";

                nomina.periodos = new SrvEnvio.Periodo[1];
                nomina.periodos[0] = periodo;

                #endregion

                #region datos trabajador               
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
                    primerNombre = "Anderson ",
                    salarioIntegral = "0",
                    segundoApellido = "marica",
                    subTipoTrabajador = "01",
                    sueldo = "500000.00",
                    tipoContrato = "1",
                    tipoIdentificacion = "31",
                    tipoTrabajador = "01"
                };
                #endregion


                request.idSoftware = "123456";
                request.nitEmpleador = NitAutorizado;
                request.tokenEnterprise = tokenEmpresa;
                request.tokenPassword = tokenAuthorizacion;

                request.nomina = nomina;


                StreamWriter MyFile = new StreamWriter(ArchivoRequest);
                XmlSerializer Serializer1 = new XmlSerializer(typeof(SrvEnvio.Request));

                Serializer1.Serialize(MyFile, request);
                MyFile.Close();



                rtxInformacion.Text += "** asincoron ** ";
                Task<SrvEnvio.Response> docRespuesta;
                docRespuesta = serviceClienteEnvio.EnviarAsync(request);
                await docRespuesta;

                if (docRespuesta.IsCompleted)
                {

                    rtxInformacion.Text += "** INICIA ** ";
                    if (docRespuesta.Result.codigo == "200")
                    {
                        StringBuilder response = new StringBuilder();
                        response.Append("AJUSTE Codigo **ES 200***: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                        response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                        response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                        response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                        response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);

                        if (docRespuesta.Result.reglasRechazoTFHKA != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                            }
                        }

                        if (docRespuesta.Result.reglasRechazoDIAN != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                            }
                        }


                        rtxInformacion.Text += response.ToString();
                    }
                    else
                    {
                        StringBuilder response = new StringBuilder();
                        response.Append("ERROR EN EL ENVIO" + Environment.NewLine);
                        response.Append("AJUSTE Codigo **NO ES 200***: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                        response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                        response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                        response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                        response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);


                        if (docRespuesta.Result.reglasRechazoTFHKA != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                            }
                        }

                        if (docRespuesta.Result.reglasRechazoDIAN != null)
                        {
                            int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                            for (int i = 0; i < nReturnMsg; i++)
                            {
                                response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                            }
                        }


                        rtxInformacion.Text += response.ToString();

                    }

                    rtxInformacion.Text += "** ENVIA ** ";

                }
                else
                {
                    rtxInformacion.Text += "** FALLo ** ";
                    StringBuilder response = new StringBuilder();
                    response.Append("AJUSTE Codigo: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                    response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                    response.Append("Cufe: " + docRespuesta.Result.cune + Environment.NewLine);
                    response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                    response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);


                    if (docRespuesta.Result.reglasRechazoTFHKA != null)
                    {
                        int nReturnMsg = docRespuesta.Result.reglasRechazoTFHKA.Count();
                        for (int i = 0; i < nReturnMsg; i++)
                        {
                            response.Append("RECHAZO TFHKA: " + docRespuesta.Result.reglasRechazoTFHKA[i].ToString() + Environment.NewLine);
                        }
                    }

                    if (docRespuesta.Result.reglasRechazoDIAN != null)
                    {
                        int nReturnMsg = docRespuesta.Result.reglasRechazoDIAN.Count();
                        for (int i = 0; i < nReturnMsg; i++)
                        {
                            response.Append("RECHAZO DIAN: " + docRespuesta.Result.reglasRechazoDIAN[i].ToString() + Environment.NewLine);
                        }
                    }


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
