   M S B A M L   `   `   `  ����D  @Collector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Collector	Collector  5   	       Collector.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Collector     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5   
   �� 5       � RowDefinitions 5   
   �� 5      �� 5      $��* �6   6)   .+�5   
   �� -   5         ��Name$ 
lstContent��5      $��
5,34,5,4.5q�6'   $��0��6;    �� View 5      �� 5      -   5        
grdContent��5      5      �� 5   
   -   5         ��Name$ Import_Button��5      $��Import Data��6(   $
��Left=�6>   $��	10,10,0,0q�6Y   $	��Top=�6l   $��75��6�    J� RenderTransformOrigin$ -0.264,-0.282&�6�   6�   �� 5   
   -   5        $ Export_Button��5      $��Export Data��6(   $
��Left=�6>   $��	90,10,0,0q�6Y   $	��Top=�6l   $��75��6�   6�   �� 5   
   -   5         ��Name$ Search_Type_ComboBox��5      $
��Left=�62   $��	253,8,0,0q�6M   $	��Top=�6`   $��98��6x   �� 5    
   -   5         ��Name$ TextFilter_TextBox��5       $
��Left=�6.   $��
356,10,0,0q�6I   	 �� TextWrapping$
	 Wrap=�6]   $	��Top=�6q   $��75��6�   6�   �� 5"   
   -   5        $ Selected_All_Button��5"      $��
Select_All��6/   $
��Left=�6D   $��
436,10,0,0q�6_   $	��Top=�6s   $��75��6�   $ 
0.06,0.264&�6�   6�   �� 5$   
   -   5        $ Delete_Button��5$      $��Delete��6(   $
��Left=�69   $��
710,10,0,0q�6T   $	��Top=�6h   $��75��6�   6�   �� 5&   
   -	   5        
 ��Name$
 Current_Content��5&      $(��"This block shows selected content.��6-   $
��Left=�6W   $��
290,10,0,0q�6r   $��1��6�   $
��Wrap=�6�   $	��Top=�6�   $ -0.227,0.533&�6�   �� 5(   
   -
   5        $
 Progress��5(      $�� ��6&   $
��Left=�6.   $��
612,10,0,0q�6I   $��1��6]   $
��Wrap=�6j   $	��Top=�6~   r� 5*   
   -   5         r�Name$ AudioPlayer��5*      $
��Left=�6,   $	��100��6G   $��
685,60,0,0q�6T   $��1��6h   $	��Top=�6u   $	��100��6�   6�   �� 5,   
   -   5        $ Record_Button��5,      $��Record��6(   $
��Left=�69   $��
221,82,0,0q�6T   $��1��6h   $	��Top=�6u   $��75��6�   6�   �� 5.   
   -   5        $ Play_Button��5.      $
��Play��6&   $
��Left=�65   $��
487,82,0,0q�6P   $��1��6d   $	��Top=�6q   $��75��6�   6�   �� 50   
   -   5        $ Terminate_Button��50      $��	Terminate��6+   $
��Left=�6?   $��487,106,0,0q�6Z   $��1��6o   $	��Top=�6|   $��75��6�   6�   �� 51   
   -   5        $ Terminate_Button2��51      $��	Terminate��6,   $
��Left=�6@   $��221,105.4,0,0q�6[   $��1��6r   $	��Top=�6   $��75��6�   $ 0.504,-0.534&�6�   6�   53      54      