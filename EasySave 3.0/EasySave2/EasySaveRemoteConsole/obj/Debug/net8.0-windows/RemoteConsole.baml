   M S B A M L   `   `   `  ����  EasySaveRemoteConsole)    #EasySaveRemoteConsole.RemoteConsole   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  EasySave Remote Console��5   	   $	��450��6)   $	��800��66   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0 0 0 10q�6.   .+�5      �� $	��IP:��6   $��Center=�6#   $��0 0 5 0q�6>   �� 5      -   5         ��Name$ IpAddressBox��5      $	��120��6,   $��0 0 10 0q�68   $��	127.0.0.1��6J   �� 5      $��Port:��6   $��Center=�6%   $��0 0 5 0q�6@   �� 5      -   5        $ PortBox��5      $��60��6'   $��0 0 10 0q�62   $
��9000��6D   �� 5      -   5        $��Connect��5      6'   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ JobGrid��5      $��1��5         AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5         SelectionMode$ Single=�5      $��0 0 0 10q�5      ��5      �� *Jobs+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��6%   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 50 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5      $
	 Name��6%   $
 * 6L    63   �� *Name+6L     5       $	 Status��6%   $	
 100 6P    65   �� *Status+6P     5!      $	 Progress��6%   $	
 100 6T    67   �� *
Progress+6T   4  .System.Windows.Controls.DataGridTemplateColumn  5#      $	 Actions��6)   $	
 150 6:      CellTemplate 5$      �� 5%      .R�5&      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5'   "   �� -   5        $	��▶��5'   )   $��30��65   $��2q�6@   $|�False��5)   !   �� 5*   "   -   5        $	��⏸��5*   )   $��30��65   $��2q�6@   $|�False��5,   !   �� 5-   "   -   5        $	��⏹��5-   )   $��30��65   $��2q�6@   $|�False��5/   !   50      51      53      55      58      59      