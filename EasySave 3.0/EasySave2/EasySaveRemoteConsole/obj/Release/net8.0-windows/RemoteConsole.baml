   M S B A M L   `   `   `  ����   EasySaveRemoteConsole)    #EasySaveRemoteConsole.RemoteConsole   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  EasySave Remote Console��$	��450��$	��800��.��� $��10q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+���  �� Orientation$ 
Horizontal=�$��0 0 0 10q�.+��� $	��IP:��$��Center=�$��0 0 5 0q��� -    ��Name$ IpAddressBox��$	��120��$��0 0 10 0q�$��	127.0.0.1���� $��Port:��$��Center=�$��0 0 5 0q��� -   $ PortBox��$��60��$��0 0 10 0q�$
��9000���� -   $��Connect��&   System.Windows.Controls.DataGrid  -     Name$ JobGrid��$��1��   AutoGenerateColumns$ False��   
IsReadOnly$
 True��   SelectionMode$ Single=�$��0 0 0 10q����� *Jobs+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 50 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Id+  $
	 Name��$
 *  �� *Name+  $	 Status��$	
 100  �� *Status+  $	 Progress��$	
 100  �� *
Progress+4  .System.Windows.Controls.DataGridTemplateColumn  $	 Actions��$	
 150    CellTemplate �� .R��� $ 
Horizontal=�$��Center=�.+��� -   $	��▶��$��30��$��2q�$|�False���� -   $	��⏸��$��30��$��2q�$|�False���� -   $	��⏹��$��30��$��2q�$|�False��