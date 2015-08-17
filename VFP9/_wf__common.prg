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
*-- Form:         _wf__common (\_wf__common.scx)
*-- ParentClass:  _wf_formbase (\forms.vcx)
*-- BaseClass:    form
*
DEFINE CLASS _wf__common AS _wf_formbase


	DoCreate = .T.
	BorderStyle = 3
	Name = "_wf__common"
	AxHost.Top = 0
	AxHost.Left = 0
	AxHost.Height = 480
	AxHost.Width = 640
	AxHost.Name = "AxHost"


	PROCEDURE Init
		LPARAMETERS appControlName as String,param1,param2,param3,param4,param5,param6,param7,param8,param9,param10,param11,param12,param13,param14,param15,param16

		this.ApplicationTypeName=appControlName
		this.Name=this.Name+"__"+CHRTRAN(TIME(),":","_")

		m_args=""
		FOR i=1 TO PCOUNT()-1 STEP 1
			m_args=m_args+IIF(LEN(m_args)=0,"",", ")+"param"+ALLTRIM(STR(i))
		ENDFOR

		m_command="DODEFAULT("+m_args+")"
		&m_command
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _wf__common
**************************************************
