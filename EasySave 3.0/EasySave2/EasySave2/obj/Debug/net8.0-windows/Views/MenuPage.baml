   M S B A M L   `   `   `  ����D  @EasySave2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:EasySave2.ViewModelsEasySave2.ViewModels  5      %clr-namespace:EasySave2	EasySave2  5          EasySave2.Views.MenuPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      1vm"clr-namespace:EasySave2.ViewModels     5      )localclr-namespace:EasySave2     5        N� Title$  MenuPage��5         BackgroundBrush N� 
Background# [  5       � DataContext 5
      �� 5   
    �� Member$ local:App.SharedViewModel��6   .��5      � #��[  6   $��20q�69    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ JobGrid��5      $��1��5         AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5         SelectionMode$ Extended=�5      	   SelectionUnit$	 FullRow=�5      $��0 0 0 10q�5       ��5      �� *
BackupJobs+
   ColumnHeaderStyle
 5#      �� 5$       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5%      ��  �� Property � �� �� Value �#44516963   �� 5&       � ��
 �����63   �� 5'       � ��$
 Bold"�63   �� 5(       � ��$ Center=�6C   �� 5)       � ��$ 6q�60   5*         Columns 5-      0  *System.Windows.Controls.DataGridTextColumn  5.      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 40 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5/        JobName# �  6%   $ * 6a    6H   �� *Name+6a     50        
SourcePath# �  6%   $ * 6j    6K   �� *
SourcePath+6j     51        DestinationPath# �  6%   $ * 6t    6P   �� *DestinationPath+6t     52        
BackupMode# �  6%   $ 80 6d    6K   �� *Mode+6d     53      $ Status��6%   $	 100 6P    65   �� *Status+6P   4  .System.Windows.Controls.DataGridTemplateColumn  55      $ Progress��6)   $	 120 6;      CellTemplate 56      �� 57      .R�58      �  � Maximum$	 100Q�6E   $��16��6S    � Value 6*   �� *
Progress+6S   59      5;        5=      $ Actions��6)   $	 140 6:    5>      �� 5?      .R�5@      ��  �� Orientation$ 
Horizontal=�6)   $��Center=�6B   .+�5A   "   �� -   5        $	��▶��5A   )   $��30��65   $��2q�6@   6K   �� 5B   "   -   5        $	��⏸��5B   )   $��30��65   $��2q�6@   6K   �� 5C   "   -   5        $	��⏹��5C   )   $��30��65   $��2q�6@   6K   5D      5E      5G      5I      �� 5L   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   $
��0 10q�6X   .+�5M      �� -   5         
 AddJob#���  5M      $	��100��68   $��5q�6D   6O   �� 5N      -   5          EditJob#���  5N      $	��100��69   $��5q�6E   6P   �� 5O      -   5          	DeleteJob#���  5O      $	��100��6;   $��5q�6G   6R   �� 5P      -   5          
ExecuteJob#���  5P      $	��100��6<   $��5q�6H   6S   �� 5Q      -	   5        $��General Settings��5Q      $	��150��60   $��5q�6<   6G   5S      5T      5U      