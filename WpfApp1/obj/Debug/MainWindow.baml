   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � 	Resources 5
       �� Color   	ColorMain �� 
TargetType  BtnStyle �� Property �� Value��%e  &	        &	 &     7   7   �� 5   
   $ #0078d4��6   6*   �� 5   
   " ��6   .�5      ��  � ��8   6+   �� 5       � ��8  6,   �� 5       � ��
 �����6+   �� 5       � ��$	 200��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   �� 5       �@�Cursor$
 Hand��6'   5      5      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   .+�5   
   � .+�5       �� -   5         ��Name$ rtxInformacion��5       $��10q�6.   
�������6:   5!      �� 5#   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��10,0,0,0q�6;   .+�5$      �� $��DOCUMENTO : ��6   $��Center=�6,   �� 5%      -   5        $��TxDoc��5%   &   $��NOM10020��6   $	��100��63   $��Center=�6?   $��30��6Z   �� 5&      -   5        $��	BtnEstado��5&      $��ESTADO��6>   $	��100��6O   #��[ 6[   �� 5'      -   5        $��BtnXML��5'      $	��XML��68   $	��100��6F   #��[ 6R   �� 5(      -   5        $��BtnPDF��5(      $	��PDF��68   $	��100��6F   #��[ 6R   5)      �� 5+   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   $��10,0,0,0q�6V   .+�5,      �� $��PATH REQUEST:��6   $
��Bold"�6-   �� 5-      -   5        $��TxPath��5-      $
��....��6&   5.      �� 50   
   $��3��6   $ 
Horizontal=�6"   $��Center=�6;   .+�51      �� -   5        $��BtnEnviarMinimos��51      $��Enviar Campos Minimos��6-   #��[ 6l   �� 52      -   5        $��BtnEnviarCompletos��52      $��Enviar Campos Completos��6/   #��[ 6r   �� 53      -	   5        $��BtnEnviarAjusteMinimos��53      $��AJUSTE Campos minimos��63   #��[ 6x   54      57      58      