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
*-- Class:        _wf_formbase (\forms.vcx)
*-- ParentClass:  _ax_host (\forms.vcx)
*-- BaseClass:    form
*-- Базовый класс для форм, предоставляющих хостинг пользовательским элементам управления .NET
*
DEFINE CLASS _wf_formbase AS _ax_host


	Top = 13
	Left = 4
	Width = 640
	DoCreate = .T.
	Caption = ".NET Windows Forms Host Form"
	Icon = "..\pictures\generic_application.ico"
	Name = "_wf_formbase"
	AxHost.Top = 0
	AxHost.Left = 0
	AxHost.Height = 480
	AxHost.Width = 640
	AxHost.Name = "AxHost"


	*-- Устанавливает состояние кнопок панели инструментов
	PROCEDURE SetToolButtonState
		LPARAMETERS button, state

		#DEFINE ApplicationToolButton_FirstRecordButton 1
		#DEFINE ApplicationToolButton_PrevRecordButton  2
		#DEFINE ApplicationToolButton_NextRecordButton  3
		#DEFINE ApplicationToolButton_LastRecordButton  4
		#DEFINE ApplicationToolButton_RefreshButton     5
		#DEFINE ApplicationToolButton_AddRecordButton   6
		#DEFINE ApplicationToolButton_EditRecordButton  7
		#DEFINE ApplicationToolButton_RemoveButton      8
		#DEFINE ApplicationToolButton_SaveButton        9
		#DEFINE ApplicationToolButton_UndoButton        10
		#DEFINE ApplicationToolButton_FindButton        11
		#DEFINE ApplicationToolButton_FindNextButton    12
		#DEFINE ApplicationToolButton_CutButton         13
		#DEFINE ApplicationToolButton_CopyButton        14
		#DEFINE ApplicationToolButton_PasteButton       15
		#DEFINE ApplicationToolButton_PrintButton       16
		#DEFINE ApplicationToolButton_ExitButton        17

		*-- ...

	ENDPROC


	*-- Обработчик нажатия кнопок панели инструментов.
	PROCEDURE OnToolButtonClick
		LPARAMETERS button as Integer 

		#DEFINE ParamType_Int		1
		#DEFINE ParamType_Double	2
		#DEFINE ParamType_String	3
		#DEFINE ParamType_Char		4
		#DEFINE ParamType_Bool		5
		#DEFINE ParamType_DateTime	6
		#DEFINE ParamType_Unknown	7

		oMethod=this.AxHost.CreateMethodDescriptor("VfpToolButtonClick_9CACAA37F68546C68EC0578463BCDFB5")
		oMethod.AddParam(button,ParamType_Int)
		this.AxHost.PerformMethod(oMethod)
	ENDPROC


	PROCEDURE OnEventProcessing
		LPARAMETERS EventDescriptor as Object

		DODEFAULT(EventDescriptor)

		DO CASE 
			CASE EventDescriptor.EventName="VfpConnectionRequired_ED37CBB9BB88499CA2D2633826835D7C"
				oBuilder=oApp.GetConnectionStringBuilder()
				m_str=oBuilder.GetConnectionString()
				EventDescriptor.SetParam("ConnectionString",m_str)
			CASE EventDescriptor.EventName="VfpToolButonsStateChangeRequired_0C9678D80B9045AFA077AF524727F4B0"
				m_value=EventDescriptor.GetParam("Buttons")
				m_isEnable=EventDescriptor.GetParam("IsEnable")
				FOR i=0 TO 31 STEP 1
					IF BITTEST(m_value,i) THEN 
						this.SetToolButtonState(i+1,m_isEnable)
					ENDIF 
				ENDFOR
				IF TYPE("oApp.oToolBar") = "O" AND !ISNULL(oApp.oToolBar) THEN
					oApp.oToolBar.Refresh()
				ENDIF
		ENDCASE 
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _wf_formbase
**************************************************
