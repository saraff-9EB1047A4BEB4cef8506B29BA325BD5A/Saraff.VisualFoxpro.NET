* ���� ���� �������� ������ ���������� Saraff.VisualFoxpro.NET
* � SARAFF SOFTWARE (����������� ������), 2015.
* Saraff.VisualFoxpro.NET - ��������� ���������: �� ������ ������������������ �� �/���
* �������� �� �� �������� ������� ����������� ������������ �������� GNU � ��� ����,
* � ����� ��� ���� ������������ ������ ���������� ������������ �����������;
* ���� ������ 3 ��������, ���� (�� ������ ������) ����� ����� �������
* ������.
* Saraff.VisualFoxpro.NET ���������������� � �������, ��� ��� ����� ��������,
* �� ���� ������ ��������; ���� ��� ������� �������� ��������� ����
* ��� ����������� ��� ������������ �����. ��������� ��. � ������� �����������
* ������������ �������� GNU.
* �� ������ ���� �������� ����� ������� ����������� ������������ �������� GNU
* ������ � ���� ����������. ���� ��� �� ���, ��.
* <http://www.gnu.org/licenses/>.)
* 
* This file is part of Saraff.VisualFoxpro.NET.
* � SARAFF SOFTWARE (Kirnazhytski Andrei), 2014.
* Saraff.VisualFoxpro.NET is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* Saraff.VisualFoxpro.NET is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Lesser General Public License for more details.
* You should have received a copy of the GNU Lesser General Public License
* along with Saraff.VisualFoxpro.NET. If not, see <http://www.gnu.org/licenses/>.
* 
* PLEASE SEND EMAIL TO:  vfp@saraff.ru.
**************************************************
*-- Class:        _wf_controlbase (\controls.vcx)
*-- ParentClass:  _ax_control (\controls.vcx)
*-- BaseClass:    container
*
DEFINE CLASS _wf_controlbase AS _ax_control


	Name = "_wf_controlbase"
	AxHost.Top = 0
	AxHost.Left = 0
	AxHost.Height = 75
	AxHost.Width = 75
	AxHost.Name = "AxHost"


	PROCEDURE OnEventProcessing
		LPARAMETERS EventDescriptor as Object

		DODEFAULT(EventDescriptor)

		DO CASE 
			CASE EventDescriptor.EventName="VfpConnectionRequired_ED37CBB9BB88499CA2D2633826835D7C"
				oBuilder=oApp.GetConnectionStringBuilder()
				m_str=oBuilder.GetConnectionString()
				EventDescriptor.SetParam("ConnectionString",m_str)
		ENDCASE 
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _wf_controlbase
**************************************************
