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
*-- Class:        EmployeePhoto (\controls.vcx)
*-- ParentClass:  _wf_controlbase (\controls.vcx)
*-- BaseClass:    container
*-- ���������� ���������
*
DEFINE CLASS EmployeePhoto AS _wf_controlbase


	Width = 150
	Height = 150
	ApplicationTypeName = "Transistor.Personnel.Common.dll!Transistor.Personnel.Common.EmployeePhoto"
	Name = "EmployeePhoto"
	AxHost.Top = 0
	AxHost.Left = 0
	AxHost.Height = 75
	AxHost.Width = 75
	AxHost.Name = "AxHost"


	*-- ���������� ���� ��� ���������� ���������
	PROCEDURE ShowPhoto
		LPARAMETERS m_tabn as Integer 

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7

		oMethodDescriptor=this.AxHost.CreateMethodDescriptor("ShowPhoto")
		oMethodDescriptor.AddParam(m_tabn,ParamType_Int)
		this.AxHost.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


	*-- ���������� ����
	PROCEDURE ResetPhoto

		oMethodDescriptor=this.AxHost.CreateMethodDescriptor("ResetPhoto")
		this.AxHost.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


ENDDEFINE
*
*-- EndDefine: EmployeePhoto
**************************************************
