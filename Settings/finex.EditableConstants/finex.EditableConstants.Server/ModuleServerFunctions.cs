﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace finex.EditableConstants.Server
{
	public class ModuleFunctions
	{
		#region	Работа с константами
		
		#region Работа со значениями констант
		
		/// <summary>
		/// Получить константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="typeValue">Тип константы.</param>
		/// <returns>Константа. Если не найдена, то null.</returns>
		public virtual finex.EditableConstants.IConstantsEntity GetConstant(string name, Sungero.Core.Enumeration typeValue)
		{
			return this.GetConstant(name, typeValue, true);
		}
		
		/// <summary>
		/// Получить константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="typeValue">Тип константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Константа. Если не найдена, то null.</returns>
		public virtual finex.EditableConstants.IConstantsEntity GetConstant(string name, Sungero.Core.Enumeration typeValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			
			if (!string.IsNullOrEmpty(name))
			{
				var constantEntity = finex.EditableConstants.ConstantsEntities.GetAll().Where(c => c.Name.Trim() == name.Trim() && c.TypeValue == typeValue).FirstOrDefault();
				if (constantEntity != null)
					return 	constantEntity;
				else
				{
					string textError = string.Format("Константа \"{0}\" - не найдена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			else
			{
				string textError = "В функцию поиска не передано имя константы";
				SendNoticeAndCreateExeption(subjectError, textError, genException);
			}
			
			return null;
		}
		
		
		#region Строковые значения
		
		/// <summary>
		/// Получить строковое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
		[Remote, Public]
		public virtual string GetValueStringByName(string name)
		{
			return this.GetValueStringByName(name, true);
		}
		
		/// <summary>
		/// Получить строковое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
		[Remote, Public]
		public virtual string GetValueStringByName(string name, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValString;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (string.IsNullOrEmpty(constantEntity.ValueString) || string.IsNullOrWhiteSpace(constantEntity.ValueString))
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					return constantEntity.ValueString;
			}
			return string.Empty;
		}
		
		/// <summary>
		/// Установить значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueStringByName(string name, string constValue)
		{
			return this.SetValueStringByName(name, constValue, true);
		}
		
		/// <summary>
		/// Установить значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueStringByName(string name, string constValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValString;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueString = constValue;
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#region Целые значения
		
		/// <summary>
		/// Получить целое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual int? GetValueIntByName(string name)
		{
			return this.GetValueIntByName(name, true);
		}
		
		/// <summary>
		/// Получить целое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual int? GetValueIntByName(string name, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValInt;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (!constantEntity.ValueInt.HasValue)
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					return constantEntity.ValueInt;
			}
			
			return null;
		}
		
		/// <summary>
		/// Установить целое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueIntByName(string name, int constValue)
		{
			return this.SetValueIntByName(name, constValue, true);
		}
		
		/// <summary>
		/// Установить целое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueIntByName(string name, int constValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValInt;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueInt = constValue;
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#region Вещественные значения
		/// <summary>
		/// Получить вещественное значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual double? GetValueDoubleByName(string name)
		{
			return this.GetValueDoubleByName(name, true);
		}
		
		/// <summary>
		/// Получить вещественное значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual double? GetValueDoubleByName(string name, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValDouble;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (!constantEntity.ValueDouble.HasValue)
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					return constantEntity.ValueDouble;
			}
			
			return null;
		}
		
		/// <summary>
		/// Установить вещественное значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueDoubleByName(string name, double constValue)
		{
			return this.SetValueDoubleByName(name, constValue, true);
		}
		
		/// <summary>
		/// Установить вещественное значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueDoubleByName(string name, double constValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValDouble;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueDouble = constValue;
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#region Логические значения
		
		/// <summary>
		/// Получить логическое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual bool? GetValueBooleanByName(string name)
		{
			return this.GetValueBooleanByName(name, true);
		}
		
		/// <summary>
		/// Получить логическое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual bool? GetValueBooleanByName(string name, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValBool;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (!constantEntity.ValueBool.HasValue)
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					return constantEntity.ValueBool;
			}
			
			return null;
		}
		
		/// <summary>
		/// Установить логическое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueBooleanByName(string name, bool constValue)
		{
			return this.SetValueBooleanByName(name, constValue, true);
		}
		
		
		/// <summary>
		/// Установить логическое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueBooleanByName(string name, bool constValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValBool;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueBool = constValue;
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#region Текстовые значения
		
		/// <summary>
		/// Получить текстовое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
		[Remote, Public]
		public virtual string GetValueTextByName(string name)
		{
			return this.GetValueStringByName(name, true);
		}
		
		/// <summary>
		/// Получить текстовое значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то string.Empty.</returns>
		[Remote, Public]
		public virtual string GetValueTextByName(string name, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValText;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (string.IsNullOrEmpty(constantEntity.ValueText) || string.IsNullOrWhiteSpace(constantEntity.ValueText))
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					return constantEntity.ValueText;
			}
			return string.Empty;
		}
		
		/// <summary>
		/// Установить значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueTextByName(string name, string constValue)
		{
			return this.SetValueStringByName(name, constValue, true);
		}
		
		/// <summary>
		/// Установить значение константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueTextByName(string name, string constValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValText;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueText = constValue;
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		
		#region Список строковых значений
		
		/// <summary>
		/// Получить список строковых значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то пустой List.</returns>
		[Remote, Public]
		public virtual List<string> GetValueListStringByName(string name)
		{
			return this.GetValueListStringByName(name, true);
		}
		
		/// <summary>
		/// Получить список строковых значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то пустой List.</returns>
		[Remote, Public]
		public virtual List<string> GetValueListStringByName(string name, bool genException)
		{
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValListString;
			var listValues = new List<string> {};
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				foreach (string constValue in constantEntity.ValueCollection.Where(c => !string.IsNullOrEmpty(c.ValueString) && !string.IsNullOrWhiteSpace(c.ValueString)).Select(c => c.ValueString))
					listValues.Add(constValue);
			}
			
			return listValues;
		}
		
		/// <summary>
		/// Добавить список строковых значений в константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="listValue">Список новых значений.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueListStringByName(string name, List<string> listValue)
		{
			return this.SetValueListStringByName(name, listValue, true);
		}
		
		/// <summary>
		/// Добавить список строковых значений в константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="listValue">Список новых значений.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueListStringByName(string name, List<string> listValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValListString;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					foreach (var newValue in listValue)
					{
						var newLine = constantEntity.ValueCollection.AddNew();
						newLine.ValueString = newValue;
					}
					constantEntity.Save();
					
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#region Список целых значений
		
		/// <summary>
		/// Получить список целочисленных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
		[Remote, Public]
		public virtual List<int> GetValueListIntByName(string name)
		{
			return this.GetValueListIntByName(name, true);
		}
		
		/// <summary>
		/// Получить список целочисленных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
		[Remote, Public]
		public virtual List<int> GetValueListIntByName(string name, bool genException)
		{
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValListInt;
			var listValues = new List<int> {};
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				foreach (int constValue in constantEntity.ValueCollection.Where(c => c.ValueInt.HasValue).Select(c => c.ValueInt))
					listValues.Add(constValue);
			}
			
			return listValues;
		}
		
		/// <summary>
		/// Добавить список целочисленных значений в константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="listValue">Список новых значений.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueListIntByName(string name, List<int> listValue)
		{
			return this.SetValueListIntByName(name, listValue, true);
		}
		
		/// <summary>
		/// Добавить список целочисленных значений в константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="listValue">Список новых значений.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueListIntByName(string name, List<int> listValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValListString;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					foreach (var newValue in listValue)
					{
						var newLine = constantEntity.ValueCollection.AddNew();
						newLine.ValueInt = newValue;
					}
					constantEntity.Save();
					
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#region Список вещественных значений
		
		/// <summary>
		/// Получить список вещественных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
		[Remote, Public]
		public virtual List<double> GetValueListDoubleByName(string name)
		{
			return this.GetValueListDoubleByName(name, true);
		}
		
		/// <summary>
		/// Получить список вещественных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то пустой List.</c>.</returns>
		[Remote, Public]
		public virtual List<double> GetValueListDoubleByName(string name, bool genException)
		{
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValListDouble;
			var listValues = new List<double> {};
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				foreach (double constValue in constantEntity.ValueCollection.Where(c => c.ValueDouble.HasValue).Select(c => c.ValueDouble))
					listValues.Add(constValue);
			}
			
			return listValues;
		}
		
		/// <summary>
		/// Добавить список вещественных значений в константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="listValue">Список новых значений.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueListIntByName(string name, List<double> listValue)
		{
			return this.SetValueListIntByName(name, listValue, true);
		}
		
		/// <summary>
		/// Добавить список вещественных значений в константу.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="listValue">Список новых значений.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueListIntByName(string name, List<double> listValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValListString;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					foreach (var newValue in listValue)
					{
						var newLine = constantEntity.ValueCollection.AddNew();
						newLine.ValueDouble = newValue;
					}
					constantEntity.Save();
					
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		
		#region Диапазон целочисленных значений
		
		/// <summary>
		/// Получить диапазон целочисленных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Структура RangeIntValues {From, By}.</returns>
		[Remote, Public]
		public virtual finex.EditableConstants.Structures.Module.IRangeIntValues GetValueRangeIntByName(string name)
		{
			return this.GetValueRangeIntByName(name, true);
		}
		
		/// <summary>
		/// Получить диапазон целочисленных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Структура RangeIntValues {From, By}.</returns>
		[Remote, Public]
		public virtual finex.EditableConstants.Structures.Module.IRangeIntValues GetValueRangeIntByName(string name, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValRangeInt;

			var rangeInt = finex.EditableConstants.Structures.Module.RangeIntValues.Create();
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (!constantEntity.ValueIntFrom.HasValue)
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					rangeInt.From = constantEntity.ValueIntFrom.Value;
				
				if (!constantEntity.ValueIntBy.HasValue)
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					rangeInt.By = constantEntity.ValueIntBy.Value;
			}
			
			return rangeInt;
		}
		
		/// <summary>
		/// Установить диапазон целочисленных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValueFrom">Новое значение константы (Значение С).</param>
		/// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueRangeIntByName(string name, int constValueFrom, int constValueBy)
		{
			return this.SetValueRangeIntByName(name, constValueFrom, constValueBy, true);
		}
		
		/// <summary>
		/// Установить диапазон целочисленных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValueFrom">Новое значение константы (Значение С).</param>
		/// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueRangeIntByName(string name, int constValueFrom, int constValueBy, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValRangeInt;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueIntFrom = constValueFrom;
					constantEntity.ValueIntBy = constValueBy;
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#region Диапазон вещественных значений
		
		/// <summary>
		/// Получить диапазон вещественных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Структура RangeDoubleValues {From, By}.</returns>
		[Remote, Public]
		public virtual finex.EditableConstants.Structures.Module.IRangeDoubleValues GetValueRangeDoubleByName(string name)
		{
			return this.GetValueRangeDoubleByName(name, true);
		}
		
		/// <summary>
		/// Получить диапазон вещественных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Структура RangeDoubleValues {From, By}.</returns>
		[Remote, Public]
		public virtual finex.EditableConstants.Structures.Module.IRangeDoubleValues GetValueRangeDoubleByName(string name, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValRangeDouble;

			var rangeDouble = finex.EditableConstants.Structures.Module.RangeDoubleValues.Create();
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (!constantEntity.ValueDoubleFrom.HasValue)
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					rangeDouble.From = constantEntity.ValueDoubleFrom.Value;
				
				if (!constantEntity.ValueDoubleBy.HasValue)
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					rangeDouble.By = constantEntity.ValueDoubleBy.Value;
			}
			
			return rangeDouble;
		}
		
		/// <summary>
		/// Установить диапазон вещественных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValueFrom">Новое значение константы (Значение С).</param>
		/// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueRangeDoubleByName(string name, double constValueFrom, double constValueBy)
		{
			return this.SetValueRangeDoubleByName(name, constValueFrom, constValueBy, true);
		}
		
		/// <summary>
		/// Установить диапазон вещественных значений константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValueFrom">Новое значение константы (Значение С).</param>
		/// <param name="constValueBy">Новое значение константы (Значение ПО).</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueRangeDoubleByName(string name, double constValueFrom, double constValueBy, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValRangeDouble;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueDoubleFrom = constValueFrom;
					constantEntity.ValueDoubleBy = constValueBy;
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		
		#region Base64 значения
		
		/// <summary>
		/// Получить значение Base64 константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <returns>Значение константы в формате Base64, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual string GetValueBase64ByName(string name)
		{
			return this.GetValueBase64ByName(name, false, true);
		}
		
		/// <summary>
		/// Получить значение Base64 константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="base64Decode">Преобразовать значение из Base64.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual string GetValueBase64ByName(string name, bool base64Decode)
		{
			return this.GetValueBase64ByName(name, base64Decode, true);
		}
		
		/// <summary>
		/// Получить значение Base64 константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="base64Decode">Преобразовать значение из Base64.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>Значение константы, если константа не найдена, то null.</returns>
		[Remote, Public]
		public virtual string GetValueBase64ByName(string name, bool base64Decode, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValBase64;

			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				if (string.IsNullOrEmpty(constantEntity.ValueBase64) || string.IsNullOrWhiteSpace(constantEntity.ValueBase64))
				{
					string textError = string.Format("Константа \"{0}\" - не заполнена, проверьте справочник \"Константы\"!", name);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
				else
					return base64Decode ? Base64Decode(constantEntity.ValueBase64) : constantEntity.ValueBase64;
			}
			
			return null;
		}
		
		/// <summary>
		/// Установить значение Base64 константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueBase64ByName(string name, string constValue)
		{
			return this.SetValueBase64ByName(name, constValue, true);
		}
		
		/// <summary>
		/// Установить значение Base64 константы.
		/// </summary>
		/// <param name="name">Имя константы.</param>
		/// <param name="constValue">Новое значение константы.</param>
		/// <param name="genException">Генерировать исключения.</param>
		/// <returns>True - если значение установлено, иначе - False.</returns>
		[Remote, Public]
		public virtual bool SetValueBase64ByName(string name, string constValue, bool genException)
		{
			string subjectError = "Произошла ошибка при обращении к общей константе";
			var typeValue = finex.EditableConstants.ConstantsEntity.TypeValue.ValBase64;
			
			var constantEntity = GetConstant(name, typeValue, genException);
			if (constantEntity != null)
			{
				try
				{
					constantEntity.ValueBase64 = Base64Encode(constValue);
					constantEntity.Save();
					return true;
				}
				catch (Exception e)
				{
					string textError = string.Format("Произошла ошибка записи в константу \"{0}\": {1}", name, e.Message);
					SendNoticeAndCreateExeption(subjectError, textError, genException);
				}
			}
			
			return false;
		}
		
		#endregion
		
		#endregion		
		
		#region Уведомления
		
		/// <summary>
		/// Отправить уведомление администраторам и сгенерировать исключение.
		/// </summary>
		/// <param name="subject">Тема.</param>
		/// <param name="text">Текст.</param>
		/// <param name="genException">Генерировать исключения.</param>
		[Remote, Public]
		public static void SendNoticeAndCreateExeption(string subject, string text, bool genException)
		{
			finex.CollectionFunctions.PublicFunctions.Module.Remote.SendNotice(subject, text);
			
			if (genException)
				throw new InvalidOperationException(text);
			else
				Logger.ErrorFormat("{0}", text);
		}
				
		#endregion
				
		#region Прочие функции
		
		/// <summary>
		/// Получить все константы
		/// </summary>
		[Remote(IsPure=true), Public]
		public static IQueryable<finex.EditableConstants.IConstantsEntity> GetConstants()
		{
			return finex.EditableConstants.ConstantsEntities.GetAll();
		}
		
		#endregion
		
		#endregion
		
		
		
		#region Общие функции
		
		/// <summary>
		/// Преобразование строки в Base64
		/// </summary>
		/// <param name="plainText">Строка</param>
		/// <returns>Значение преобразованное в Base64</returns>
		[Remote(IsPure = true)]
		public static string Base64Encode(string plainText) {
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		
		/// <summary>
		/// Преобразование из Base64 в строку
		/// </summary>
		/// <param name="base64EncodedData">Строка в Base64</param>
		/// <returns>Значение преобразованное из Base64 в строку UTF8</returns>
		[Remote(IsPure = true)]
		public static string Base64Decode(string base64EncodedData) {
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
		
		#endregion
	}
}