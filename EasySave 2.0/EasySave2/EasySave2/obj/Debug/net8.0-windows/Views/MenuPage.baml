   M S B A M L   `   `   `  ����D  @EasySave2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:EasySave2.ViewModelsEasySave2.ViewModels  5          EasySave2.Views.MenuPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      1vm"clr-namespace:EasySave2.ViewModels     5        N� Title$  MenuPage��5         BackgroundBrush N� 
Background# [  5       � DataContext 5      (   "EasySave2.ViewModels.MenuViewModel  5	   
   .��5      � #��[  6   $��20q�69    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ JobGrid��5      $��0��5         AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5         SelectionMode$ Extended=�5         SelectionUnit$ FullRow=�5      $��0 0 0 10q�5      ��5      �� *
BackupJobs+	   ColumnHeaderStyle	 5      �� 5      
 �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"
  6   .�5       ��  �� Property � �� �� Value �#44516963   �� 5!       � ��
 �����63   �� 5"       � ��$
 Bold"�63   �� 5#       � ��$ Center=�6C   �� 5$       � ��$ 6q�60   5%         Columns 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 40 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5*        JobName# �  6%   $ * 6a    6H   �� *Name+6a     5+        
SourcePath# �  6%   $ * 6j    6K   �� *
SourcePath+6j     5,        DestinationPath# �  6%   $ * 6t    6P   �� *DestinationPath+6t     5-        
BackupMode# �  6%   $ 80 6d    6K   �� *Mode+6d     5.      $ Status��6%   $	 100 6P    65   �� *Status+6P   50      �� 54   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $
��0 10q�6X   .+�55      �� -   5         
 AddJob#���  55      $	��100��68   $��5q�6D   6O   �� 56      -   5          EditJob#���  56      $	��100��69   $��5q�6E   6P   �� 57      -   5          	DeleteJob#���  57      $	��100��6;   $��5q�6G   6R   �� 58      -   5          
ExecuteJob#���  58      $	��100��6<   $��5q�6H   6S   59      5:      5;      