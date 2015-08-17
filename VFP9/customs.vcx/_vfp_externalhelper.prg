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
*-- Class:        _vfp_externalhelper (\customs.vcx)
*-- ParentClass:  custom
*-- BaseClass:    custom
*
DEFINE CLASS _vfp_externalhelper AS custom


	Name = "_vfp_externalhelper"


	*-- Выполнает обработку вызова компонента приложения.
	PROCEDURE InvokeAppComponent
		LPARAMETERS EventDescriptor as Object

		m_result=-1
		m_type=EventDescriptor.GetParam("ComponentType")

		DO CASE 
			CASE m_type="Form"
				m_args=""
				FOR i=1 TO EventDescriptor.GetParam("ArgsCount") STEP 1
					m_args=m_args+IIF(LEN(m_args)=0," WITH ",", ")+"EventDescriptor.GetParam('Arg"+ALLTRIM(STR(i))+"')"
				ENDFOR

				m_componentName=EventDescriptor.GetParam("ComponentName")
				DO CASE 
					CASE m_componentName="_wf__report"
						m_command="DO FORM "+m_componentName+m_args
					OTHERWISE 
						m_command="DO FORM "+m_componentName+m_args+" TO m_result"
				ENDCASE 
				&m_command
			CASE m_type="Object"
				m_instance=EventDescriptor.GetParam("InstanceName")
				m_command="PUBLIC "+m_instance
				&m_command

				m_args=""
				FOR i=1 TO EventDescriptor.GetParam("ArgsCount") STEP 1
					m_args=m_args+", "+"EventDescriptor.GetParam('Arg"+ALLTRIM(STR(i))+"')"
				ENDFOR

				m_command=m_instance+"=CREATEOBJECT('"+EventDescriptor.GetParam("ComponentName")+"'"+m_args+")"
				&m_command
			CASE m_type="Method"
				m_method=EventDescriptor.GetParam("MethodName")
				m_component=EventDescriptor.GetParam("ComponentName")
				DO CASE
					CASE m_method="RELEASE_DB5D35D29C4C46C0B5EC0450FDA3351B"
						m_command="RELEASE "+m_component
					OTHERWISE
						m_args=""
						FOR i=1 TO EventDescriptor.GetParam("ArgsCount") STEP 1
							m_args=m_args+IIF(LEN(m_args)=0,"",", ")+"EventDescriptor.GetParam('Arg"+ALLTRIM(STR(i))+"')"
						ENDFOR
						m_command=m_component+"."+m_method+"("+m_args+")"
				ENDCASE
				&m_command
			CASE m_type="Property"
				m_property=EventDescriptor.GetParam("ComponentName")+"."+EventDescriptor.GetParam("PropertyName")
				m_method=EventDescriptor.GetParam("Method")
				DO CASE
					CASE m_method="Get"
						m_command="m_result="+m_property
					CASE m_method="Set"
						m_command=m_property+"=EventDescriptor.GetParam('Arg1')"
				ENDCASE
				&m_command
			CASE m_type="Program"
				m_args=""
				FOR i=1 TO EventDescriptor.GetParam("ArgsCount") STEP 1
					m_args=m_args+IIF(LEN(m_args)=0," WITH ",", ")+"EventDescriptor.GetParam('Arg"+ALLTRIM(STR(i))+"')"
				ENDFOR

				m_command="DO "+EventDescriptor.GetParam("ComponentName")+m_args
				&m_command
			CASE m_type="Code"
				m_args=""
				FOR i=1 TO EventDescriptor.GetParam("ArgsCount") STEP 1
					m_args=m_args+", "+"EventDescriptor.GetParam('Arg"+ALLTRIM(STR(i))+"')"
				ENDFOR

				m_command="m_result=EXECSCRIPT(EventDescriptor.GetParam('VfpCode')"+m_args+")"
				&m_command
		ENDCASE 
		EventDescriptor.SetParam("ReturnValue",m_result)
	ENDPROC


ENDDEFINE
*
*-- EndDefine: _vfp_externalhelper
**************************************************
