   M S B A M L   `   `   `  ����  	EasySave2"    EasySave2.Views.AddJobDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	    
  AddJob  � Title#  �   5   	   $	��450��6*   $	��400��67    � WindowStartupLocation$ CenterScreen=�5   	     BackgroundBrush#��[ 5   	   .��5      �� $
��Auto=�6   .��5	   
   �� $��20q�6   .+�5      ��   JobName#���  6     	TextBrush#��[ 69   �� 5      -   5         ��Name$ 	NameInput��5      $	��0 5q�6)   �� 5        
SourcePath#���  6   #��[ 6<   �� 5      -   5        $ SourceInput��5      $	��0 5q�6+   �� 5        DestinationPath#���  6   #��[ 6A   �� 5      -   5        $ 	DestInput��5      $	��0 5q�6)   �� 5        
BackupMode#���  6   #��[ 6<   �� 5      -   5         ��Name$ 
ModeSelect��5      $	��0 5q�6+    �� SelectedIndex$ 0��68   ���#8E2DE2
�������.@�5      �� $
��Full��6   �� 5      $
��Diff��6   5      �� 5      -   5         ��Name$ EncryptCheckbox��5      $��Encrypt Files (XOR)��5      $��0 10 0 0q�5      
�������5!      �� 5#      -   5         ��Name$ ExtensionInputPanel��5#      $y�	Collapsed=�66   .+�5$      �� -   5         ��Name$ ExtensionsLabel��5$      $.��(Extensions to Encrypt (e.g. .txt,.pdf,*)��5%      
�������$��0 5 0 0q�5'      �� 5(      -   5        $ ExtensionsInput��5(      $��25��5)      ���#FFF���#000$	��0 5q�5,      5-      �� 51      $��Log Format:��6   $��	10 10 0 0q�6+   �� 52      -	   5        $ LogFormatSelect��52      $ 0��60   $	��0 5q�6B   .@�53      �� $
��json��6   �� 54      $	��xml��6   55      �� 58      $*��$Blocked Software (e.g., notepad.exe)��6   $��0,10,0,4q�6D   �� 59      -
   5        $ SoftwarePackageBox��59      $��30��5:      $��5q�5;      ���#FFF���#0005=      �� 5?      -   5        #���   5?      $��
10 15 10 0q�5@      $	��100��5A      $��Center=�5B      5C      5D      5E      5F      