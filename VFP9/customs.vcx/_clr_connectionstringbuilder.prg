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
*-- Class:        _clr_connectionstringbuilder (\customs.vcx)
*-- ParentClass:  _clr_host (\customs.vcx)
*-- BaseClass:    custom
*-- Provides a simple way to create and manage the contents of connection strings.
*
DEFINE CLASS _clr_connectionstringbuilder AS _clr_host


	ApplicationTypeName = "Transistor.Personnel.Common.dll!Transistor.Personnel.Common.ConnectionStringBuilder"
	Name = "_clr_connectionstringbuilder"


	*-- Adds an entry with the specified key and value.
	PROCEDURE Add
		LPARAMETERS keyword as String,value_ as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("Add")
		oMethodDescriptor.AddParam(keyword,ParamType_String)
		oMethodDescriptor.AddParam(value_,ParamType_String)
		this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


	*-- Clears the contents of instance.
	PROCEDURE Clear

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7

		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("Clear")
		this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


	*-- Gets the connection string associated with the  ConnectionStringBuilder.
	PROCEDURE GetConnectionString

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("GetConnectionString")
		_res=this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
		RETURN _res
	ENDPROC


	*-- Sets the connection string associated with the  ConnectionStringBuilder.
	PROCEDURE SetConnectionString
		LPARAMETERS value_ as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("SetConnectionString")
		oMethodDescriptor.AddParam(value_,ParamType_String)
		this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


	*-- Determines whether the ConnectionStringBuilder contains a specific key.
	PROCEDURE ContainsKey
		LPARAMETERS keyword as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("ContainsKey")
		oMethodDescriptor.AddParam(keyword,ParamType_String)
		_res=this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
		RETURN _res
	ENDPROC


	*-- Gets the current number of keys that are contained within the ConnectionStringBuilder.
	PROCEDURE GetCount

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("GetCount")
		_res=this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
		RETURN _res
	ENDPROC


	*-- Gets the name of the ODBC driver associated with the connection.
	PROCEDURE GetDriver

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("GetDriver")
		_res=this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
		RETURN _res
	ENDPROC


	*-- Sets the name of the ODBC driver associated with the connection.
	PROCEDURE SetDriver
		LPARAMETERS value_ as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("SetDriver")
		oMethodDescriptor.AddParam(value_,ParamType_String)
		this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


	*-- Gets the name of the data source name (DSN) associated with the connection.
	PROCEDURE GetDsn

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("GetDsn")
		_res=this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
		RETURN _res
	ENDPROC


	*-- Sets the name of the data source name (DSN) associated with the connection.
	PROCEDURE SetDsn
		LPARAMETERS value_ as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("SetDsn")
		oMethodDescriptor.AddParam(value_,ParamType_String)
		this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


	*-- Removes the entry with the specified key from the ConnectionStringBuilder instance.
	PROCEDURE Remove
		LPARAMETERS keyword as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("Remove")
		oMethodDescriptor.AddParam(keyword,ParamType_String)
		this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


	*-- Gets the value associated with the specified key.
	PROCEDURE GetValue
		LPARAMETERS keyword as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("GetValue")
		oMethodDescriptor.AddParam(keyword,ParamType_String)
		_res=this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
		RETURN _res
	ENDPROC


	*-- Sets the value associated with the specified key.
	PROCEDURE SetValue
		LPARAMETERS keyword as String,value_ as String

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7


		oMethodDescriptor=this.oComponent.CreateMethodDescriptor("SetValue")
		oMethodDescriptor.AddParam(keyword,ParamType_String)
		oMethodDescriptor.AddParam(value_,ParamType_String)
		this.oComponent.PerformMethod(oMethodDescriptor)
		RELEASE oMethodDescriptor
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _clr_connectionstringbuilder
**************************************************
