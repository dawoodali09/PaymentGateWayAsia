using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace PaymentGateWayAsia.DAL
{
    [Serializable]
    public class DataObject
    {
        
      
        private ArrayList _fields = new ArrayList();
        private List<DataObjectRow> _rows = new List<DataObjectRow>();
        private bool firstrow = true;

        public DataObject(IDataReader objDataReader)
        {
            DataTable dc = objDataReader.GetSchemaTable();

            if (!objDataReader.IsClosed)
            {
                while (objDataReader.Read())
                {
                    object[] dataRow = new object[objDataReader.FieldCount];
                    int i = 0;

                    if (firstrow)
                    {
                        foreach (DataRow row in dc.Rows)
                        {

                            _fields.Add((string)row["columnname"]);
                            i++;
                        }

                        firstrow = false;
                    }

                    _rows.Add(new DataObjectRow(ref _fields, objDataReader));
                }

                dc.Clear();
                objDataReader.Close();
                firstrow = true;
            }
            dc.Clear();
            objDataReader.Close();
        }

        public List<DataObjectRow> Rows
        {
            get { return _rows; }
        }
    }

    public class DataObjectRow
    {
        private ArrayList _values = new ArrayList();
        private ArrayList _fields = null;

        public DataObjectRow(ref ArrayList fields, IDataReader values)
        {
            _fields = fields;

            int i = 0;
            while (i < values.FieldCount)
            {
                _values.Add(values.GetValue(i));
                i++;
            }

        }

        public object Get(string Key)
        {
            string lowerKey = Key.ToLowerInvariant();
            int counter = 0;
            while (counter < _fields.Count)
            {
                if (_fields[counter].ToString().ToLowerInvariant() == lowerKey)
                    return _values[counter];
                counter++;
            }
            return null;
        }

        public Boolean HasValue(string Key)
        {
            string lowerKey = Key.ToLowerInvariant();
            int counter = 0;
            while (counter < _fields.Count)
            {
                if (_fields[counter].ToString().ToLowerInvariant() == lowerKey)
                    if (_values[counter] != null)
                        if (_values[counter] != DBNull.Value)
                            return true;
                counter++;
            }
            return false;
        }

        //public object GetValue(string Key, Delegate DataMember)
        //{
        //    string lowerKey = Key.ToLowerInvariant();
        //    object objDataValue = null;
        //    int cnt = 0;
        //    while (cnt < _fields.Count && objDataValue == null)
        //    {
        //        if (((string)_fields[cnt]).ToLowerInvariant() == lowerKey)
        //            if (_values[cnt] != null)
        //                if (_values[cnt] != DBNull.Value)
        //                    objDataValue = _values[cnt];
        //        cnt++;
        //    }

        //    return Convert.ChangeType(objDataValue, DataMember.GetType());
        //}

    }
}