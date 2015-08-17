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

namespace Saraff.VisualFoxpro.Odbc {

    [Flags]
    public enum VfpToolButons {

        /// <summary>
        /// Перейти к первой записи.
        /// </summary>
        FirstRecordButton=0x0001,

        /// <summary>
        /// Перейти к предыдущей записи.
        /// </summary>
        PrevRecordButton=0x0002,

        /// <summary>
        /// Перейти к следующей записи.
        /// </summary>
        NextRecordButton=0x0004,

        /// <summary>
        /// Перейти к последней записи.
        /// </summary>
        LastRecordButton=0x0008,

        /// <summary>
        /// Обновить содержимое.
        /// </summary>
        RefreshButton=0x0010,

        /// <summary>
        /// Добавить запись.
        /// </summary>
        AddRecordButton=0x0020,

        /// <summary>
        /// Редактировать запись.
        /// </summary>
        EditRecordButton=0x0040,

        /// <summary>
        /// Удалить запись.
        /// </summary>
        RemoveRecordButton=0x0080,

        /// <summary>
        /// Сохранить.
        /// </summary>
        SaveButton=0x0100,

        /// <summary>
        /// Убрать изменения.
        /// </summary>
        UndoButton=0x0200,

        /// <summary>
        /// Поиск.
        /// </summary>
        FindButton=0x0400,

        /// <summary>
        /// Искать далее.
        /// </summary>
        FindNextButton=0x0800,

        /// <summary>
        /// Вырезать
        /// </summary>
        CutButton=0x1000,

        /// <summary>
        /// Копировать.
        /// </summary>
        CopyButton=0x2000,

        /// <summary>
        /// Вставить.
        /// </summary>
        PasteButton=0x4000,

        /// <summary>
        /// Печать.
        /// </summary>
        PrintButton=0x8000,

        /// <summary>
        /// Закрыть.
        /// </summary>
        ExitButton=0x10000
    }
}
