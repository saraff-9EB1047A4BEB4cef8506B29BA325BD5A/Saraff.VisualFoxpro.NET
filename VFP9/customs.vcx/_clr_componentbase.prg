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
*-- Class:        _clr_componentbase (\customs.vcx)
*-- ParentClass:  _clr_host (\customs.vcx)
*-- BaseClass:    custom
*
DEFINE CLASS _clr_componentbase AS _clr_host


	Name = "_clr_componentbase"


	PROCEDURE OnEventProcessing
		LPARAMETERS EventDescriptor as Object

		DODEFAULT(EventDescriptor)

		DO CASE
			CASE EventDescriptor.EventName="VfpConnectionRequired_448FCAB8513A4E5996E775521DC76FD5"
				oBuilder=oApp.GetConnectionStringBuilder()
				m_str=oBuilder.GetConnectionString()
				EventDescriptor.SetParam("ConnectionString",m_str)
		ENDCASE 
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _clr_componentbase
**************************************************
