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
*-- Class:        _ax_host (\forms.vcx)
*-- ParentClass:  form
*-- BaseClass:    form
*
DEFINE CLASS _ax_host AS form


	Top = 13
	Left = 4
	Height = 480
	Width = 640
	DoCreate = .T.
	Caption = "AxHost"
	Icon = "\code_component.ico"
	*-- Возвращает или устанавливает имя пользовательского элемента управления (Имя имеет формат: "имя файла сборки"!"полное имя элемента управления". Например, MyAssembly.dll!MyNamespace.MyControl).
	ApplicationTypeName = ""
	*-- Возвращаемое значение.
	ReturnValue = .F.
	Name = "_ax_host"


	ADD OBJECT AxHost AS olecontrol WITH ;
		Top = 0, ;
		Left = 0, ;
		Height = 480, ;
		Width = 640, ;
		Anchor = 15, ;
		Name = "AxHost"


	*-- Обработчик вызова компонентов приложения
	PROCEDURE OnInvokeAppComponent
		LPARAMETERS EventDescriptor as Object

		oHelper=this.GetExternalHelper()
		oHelper.InvokeAppComponent(EventDescriptor)
	ENDPROC


	*-- Устанавливает текущий обработчик событий.
	HIDDEN PROCEDURE CreateEventHandler
		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7

		this.AddObject("oEventHandler","_clr_EventHandler")

		oMethod=this.AxHost.CreateMethodDescriptor("VfpSetEventHandler_23469B5EFE5C4EB5B250FD5DF5C7869D")
		oHandler=this.oEventHandler.oComponent
		oMethod.AddParam(oHandler,ParamType_Unknown)
		this.AxHost.PerformMethod(oMethod)
	ENDPROC


	*-- Обрабатывает события.
	PROCEDURE OnEventProcessing
		LPARAMETERS EventDescriptor as Object

		DO CASE 
			CASE EventDescriptor.EventName="VfpExternalRequired_E579B5882EA4411CA4D5DE57E72D0F3B"
				this.OnInvokeAppComponent(EventDescriptor)
			CASE EventDescriptor.EventName="VfpErrorHandlerRequired_B96C8831F80941689F8F05DC019E453F"
				m_type=EventDescriptor.GetParam("ExceptionType")
				m_message=EventDescriptor.GetParam("Message")
				DO CASE 
					CASE m_type="Warning"
						MESSAGEBOX(m_message,48,"Управление персоналом")
					OTHERWISE 
						MESSAGEBOX(m_message,16,"Управление персоналом")
				ENDCASE 
		ENDCASE 
	ENDPROC


	*-- Возвращает экземпляр _vfp_ExtrnalHelper.
	HIDDEN PROCEDURE GetExternalHelper

		IF TYPE("this.oExternalHelper") != "O" OR ISNULL(this.oExternalHelper) THEN
			this.AddObject("oExternalHelper","_vfp_ExternalHelper")
		ENDIF 

		RETURN this.oExternalHelper
	ENDPROC


	PROCEDURE Unload
		DODEFAULT()

		RETURN this.ReturnValue
	ENDPROC


	PROCEDURE Destroy
		this.AxHost.Dispose()

		IF TYPE("this.oEventHandler") = "O" AND !ISNULL("this.oEventHandler") THEN
			this.RemoveObject("oEventHandler")
		ENDIF

		IF TYPE("this.oExternalHelper") = "O" AND !ISNULL(this.oExternalHelper) THEN
			this.RemoveObject("oExternalHelper")
		ENDIF 

		DODEFAULT()
	ENDPROC


	PROCEDURE Init
		LPARAMETERS param1,param2,param3,param4,param5,param6,param7,param8,param9,param10,param11,param12,param13,param14,param15,param16

		IF PCOUNT()>0 THEN 
			this.WindowType=1
		ENDIF 
		FOR i=1 TO PCOUNT() STEP 1
			m_param="param"+ALLTRIM(STR(i))
			this.AxHost.AddComponentParameter(&m_param)
		ENDFOR 

		this.AxHost.WorkingDirectory=FULLPATH(CURDIR())+"DLL"
		this.AxHost.ApplicationTypeName=this.ApplicationTypeName
		this.AxHost.Load()

		this.CreateEventHandler()

		DODEFAULT()
	ENDPROC


	PROCEDURE AxHost.FireEvent
		*** ActiveX Control Event ***
		LPARAMETERS eventid

		SET STEP ON 
		*!* Для обеспечения возможности передачи событий в vfp-приложение необходимо использовать атрибут 
		*!* Saraff.VisualFoxpro.VfpProcessedAttribute

		this.Parent.OnEventProcessing(eventid)
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _ax_host
**************************************************
