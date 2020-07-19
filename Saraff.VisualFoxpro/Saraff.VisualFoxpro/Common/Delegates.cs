/* Этот файл является частью библиотеки Saraff.VisualFoxpro.NET
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
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Saraff.VisualFoxpro.Common {
    public delegate void Action();
    public delegate void Action<T>(T arg);
    public delegate void Action<T1,T2>(T1 arg1,T2 arg2);
    public delegate void Action<T1,T2,T3>(T1 arg1,T2 arg2,T3 arg3);
    public delegate void Action<T1,T2,T3,T4>(T1 arg1,T2 arg2,T3 arg3,T4 arg4);
    public delegate void Action<T1,T2,T3,T4,T5>(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5);
    public delegate void Action<T1,T2,T3,T4,T5,T6>(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6);
    public delegate TResult Func<TResult>();
    public delegate TResult Func<T,TResult>(T arg);
    public delegate TResult Func<T1,T2,TResult>(T1 arg1,T2 arg2);
    public delegate TResult Func<T1,T2,T3,TResult>(T1 arg1,T2 arg2,T3 arg3);
    public delegate TResult Func<T1,T2,T3,T4,TResult>(T1 arg1,T2 arg2,T3 arg3,T4 arg4);
    public delegate TResult Func<T1,T2,T3,T4,T5,TResult>(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5);
    public delegate TResult Func<T1,T2,T3,T4,T5,T6,TResult>(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6);
}
