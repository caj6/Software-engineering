   M S B A M L   `   `   `  ����  	EasySave2"    EasySave2.Views.AddJobDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	    
  AddJob  � Title#  �   5   	   $	��450��6*   $	��400��67    � WindowStartupLocation$ CenterScreen=�5   	     BackgroundBrush#��[ 5   	   .��5      �� $
��Auto=�6   .��5	   
   �� $��20q�6   .+�5      ��   JobName#���  6     	TextBrush#��[ 69   �� 5      -   5         ��Name$ 	NameInput��5      $	��0 5q�6)   �� 5        
SourcePath#���  6   #��[ 6<   �� 5       �� Orientation$ 
Horizontal=�6   $	��0 5q�62   .+�5      �� -   5        $ SourceInput��5      $	��250��6/   $��0 0 5 0q�6;   �� 5      -   5        $
��📁��5      $��30��6&   61   5      �� 5        DestinationPath#���  6   #��[ 6A   �� 5      $ 
Horizontal=�6   $	��0 5q�62   .+�5      �� -   5        $ 	DestInput��5      $	��250��6-   $��0 0 5 0q�69   �� 5      -   5        $
��📁��5      $��30��6&   61   5      �� 5        
BackupMode#���  6   #��[ 6<   �� 5      -   5         ��Name$ 
ModeSelect��5      $	��0 5q�6+    �� SelectedIndex$ 0��68   ���#8E2DE2
�������.@�5      �� $
��Full��6   �� 5      $
��Diff��6   5      �� 5$      $+��%Priority Extensions (e.g. .pdf,.docx)��6   
�������$��0 5 0 0q�6X   �� 5%      -   5        $ PriorityExtensionsInput��5%      $��25��5&      ���#FFF���#000$	��0 5q�5)      �� 5.      $*��$Blocked Software (e.g., notepad.exe)��6   $��0,10,0,4q�6D   �� 5/      -   5        $ SoftwarePackageBox��5/      $��30��50      $��5q�51      ���#FFF���#00053      �� 55      -	   5        #���   55      $��
10 15 10 0q�56      $	��100��57      $��Center=�58      59      5:      5;      5<      