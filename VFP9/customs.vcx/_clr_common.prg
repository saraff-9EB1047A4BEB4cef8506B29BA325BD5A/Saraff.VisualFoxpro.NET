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
*-- Class:        _clr_common (\customs.vcx)
*-- ParentClass:  _clr_componentbase (customs.vcx)
*-- BaseClass:    custom
*
DEFINE CLASS _clr_common AS _clr_componentbase


	Name = "_clr_common"


	PROCEDURE InvokeMethod
		LPARAMETERS MethodName as String,Param1,Param2,Param3,Param4,Param5,Param6,Param7,Param8,Param9,Param10,Param11,Param12,Param13,Param14,Param15,Param16

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7

		oMethodDescriptor=this.oComponent.CreateMethodDescriptor(methodName)

		FOR i=1 TO PCOUNT()-1 STEP 1
			m_param="m_paramValue=param"+ALLTRIM(STR(i))
			&m_param
			oMethodDescriptor.AddParam(m_paramValue,ParamType_Unknown)
		ENDFOR

		_res=this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor

		RETURN _res
	ENDPROC


	PROCEDURE Init
		LPARAMETERS AppComponentName as String

		this.ApplicationTypeName=AppComponentName

		DODEFAULT()
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _clr_common
**************************************************
