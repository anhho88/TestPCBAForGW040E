   M S B A M L   `   `   `  ����L  HTestPCBAForGW040E, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:TestPCBAForGW040E.UserControlsTestPCBAForGW040E.UserControls  5      0    *TestPCBAForGW040E.UserControls.ucConfigDUT   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:TestPCBAForGW040E.UserControls     5      -   5        .��5
      ��   �� CornerRadius$  5,5,5,5��6   ���	#FFFAFAFA._�5   
   �  � RowDefinitions 5      �� 5      $��* �6    �� 5      $��50 �6    .+�5      �� $��0��6   .��5      �� $��10,10,10,10q�6   .+�5      �� $��15(�6   $
��Bold"�6*   $��
SerialPort��6<   6S   �� 5      $��
Port Name:��6   $��13(�61   $��0,10,0,0q�6?   6S   �� 5      -   5         ��Name$ cbbPortName��5      $��13(�64   $
��Bold"�6B   6V   �� 5      $��
Baud Rate:��6   $��13(�61   $��0,10,0,0q�6?   6S   �� 5      -   5        $ cbbBaudRate��5      $��13(�64   $
��Bold"�6B   6V   �� 5      $��15(�6   $
��Bold"�6*   $��-��6<   6J   �� 5      $��15(�6   $
��Bold"�6*   $��
Properties��6<   6S   �� 5      $��IP Address:��6   $��13(�62   $��0,10,0,0q�6@   6T   �� 5      $��13(�6   $
��Bold"�6,   ��6>   ��  �� Path$ DUT_IPAddress� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�6�   �� 5      $��User:��6   $��13(�6,   $��0,10,0,0q�6:   6N   �� 5      $��13(�6   $
��Bold"�6,   ��6>   �� $ DUT_User�$ PropertyChanged=�$ TwoWay=�6�   �� 5      $��	Password:��6   $��13(�60   $��0,10,0,0q�6>   6R   �� 5       $��13(�6   $
��Bold"�6,   ��6>   �� $ DUT_Pass�$ PropertyChanged=�$ TwoWay=�6�   �� 5!      $,��&The first 6 digits of the MAC Address:��6   $��13(�6M   $��0,10,0,0q�6[   6o   �� 5"      $��13(�6   $
��Bold"�6,   ��6>   �� $ DUT_MACFormat�$ PropertyChanged=�$ TwoWay=�6�   �� 5#      $"��Firmware Path (tclinux.bin):��6   $��13(�6C   $��0,10,0,0q�6Q   6e   �� 5$      $��13(�6   $
��Bold"�6,    �� 
IsReadOnly . ��6P   �� $ FWPath�$ PropertyChanged=�$ TwoWay=�6�   �� 5%      -   5        $
��Left=�5%   2   $��0,5,0,0q�6M   $��25��6^   $��70��6j   .��6u   	browser6~   5&      5'      � 5(      $��1��6   ���	#FF777777.+�5)      � $��Right=�6   $��5,5,10,5q�68   .+�5*      �� -   5        $��15(�5*   2   $��10,0,0,0q�6A   $��40��6S   $��80��6_   .��6j   Save6p   �� 5+      -   5        $��15(�5+   2   $��10,0,0,0q�6@   $��40��6R   $��80��6^   .��6i   	Default6r   5,      5-      5.      5/      51      