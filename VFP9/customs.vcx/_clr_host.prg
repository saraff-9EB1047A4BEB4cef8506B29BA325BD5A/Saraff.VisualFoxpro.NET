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
*-- Class:        _clr_host (\customs.vcx)
*-- ParentClass:  custom
*-- BaseClass:    custom
*
DEFINE CLASS _clr_host AS custom


	*-- Имя CLR компонента, размещаемого в контейнере.
	ApplicationTypeName = ""
	oComponent = .F.
	Name = "_clr_host"


	*-- Обработчик собитий от CLR компонента.
	PROCEDURE OnEventProcessing
		LPARAMETERS EventDescriptor as Object

		DO CASE 
			CASE EventDescriptor.EventName="VfpExternalRequired_6BD50AADD7FD4919A200FBC48E9CC28A"
				this.OnInvokeAppComponent(EventDescriptor)
			CASE EventDescriptor.EventName="VfpErrorHandlerRequired_14173D436F344779B521DC61F955F7BD"
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


	*-- Возвращает экземпляр _vfp_ExternalHelper.
	HIDDEN PROCEDURE GetExternalHelper

		IF TYPE("this.oExternalHelper") != "O" OR ISNULL(this.oExternalHelper) THEN
			this.AddObject("oExternalHelper","_vfp_ExternalHelper")
		ENDIF 

		RETURN this.oExternalHelper
	ENDPROC


	*-- Выполняет обработку вызова внешнего кода.
	PROCEDURE OnInvokeAppComponent
		LPARAMETERS EventDescriptor as Object

		oHelper=this.GetExternalHelper()
		oHelper.InvokeAppComponent(EventDescriptor)
	ENDPROC


	PROCEDURE Destroy
		IF TYPE("this.oComponent") = "O" THEN
			this.oComponent.Dispose()
			RELEASE oComponent
		ENDIF

		IF TYPE("this.oComponentEvents") = "O" THEN
			this.RemoveObject("oComponentEvents")
		ENDIF
	ENDPROC


	PROCEDURE Init
		this.oComponent=CREATEOBJECT("Saraff.AxHost.AxHostComponent")
		this.AddObject("oComponentEvents","IAxHostEvents")
		IF TYPE("this.oComponent") = "O" AND TYPE("this.oComponentEvents") = "O" THEN
			EVENTHANDLER(this.oComponent,this.oComponentEvents)
		ENDIF

		this.oComponent.WorkingDirectory=FULLPATH(CURDIR())+"DLL"
		this.oComponent.ApplicationTypeName=this.ApplicationTypeName 
		this.oComponent.Load()
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _clr_host
**************************************************
