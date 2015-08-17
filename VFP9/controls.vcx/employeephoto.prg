* Этот файл является частью библиотеки Saraff.VisualFoxpro.NET
* © SARAFF SOFTWARE (Кирножицкий Андрей), 2015.
* Saraff.VisualFoxpro.NET - свободная программа: вы можете перераспространять ее и/или
* изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
* в каком она была опубликована Фондом свободного программного обеспечения;
* либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
* версии.
* Saraff.VisualFoxpro.NET распространяется в надежде, что она будет полезной,
* но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
* или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
* общественной лицензии GNU.
* Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
* вместе с этой программой. Если это не так, см.
* <http://www.gnu.org/licenses/>.)
* 
* This file is part of Saraff.VisualFoxpro.NET.
* © SARAFF SOFTWARE (Kirnazhytski Andrei), 2014.
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
*-- Фотография работника
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


	*-- Отображает фото для указанного работника
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


	*-- Сбрасывает фото
	PROCEDURE ResetPhoto

		oMethodDescriptor=this.AxHost.CreateMethodDescriptor("ResetPhoto")
		this.AxHost.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


ENDDEFINE
*
*-- EndDefine: EmployeePhoto
**************************************************
