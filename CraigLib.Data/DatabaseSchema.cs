using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CraigLib.Data
{
    [HelpKeyword("vs.data.DataSet")]
    [ToolboxItem(true)]
    [XmlRoot("DatabaseSchema")]
    [DesignerCategory("code")]
    [XmlSchemaProvider("GetTypedDataSetSchema")]
    [Serializable]
    public class DatabaseSchema : DataSet
    {
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void ComponentDataRowChangeEventHandler(object sender, ComponentDataRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void ComponentRowChangeEventHandler(object sender, ComponentRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DbColumnRowChangeEventHandler(object sender, DbColumnRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DbConstraintRowChangeEventHandler(object sender, DbConstraintRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DbFKeyRowChangeEventHandler(object sender, DbFKeyRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DbGroupRowChangeEventHandler(object sender, DbGroupRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DbIndexRowChangeEventHandler(object sender, DbIndexRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DbTableRowChangeEventHandler(object sender, DbTableRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DbViewRowChangeEventHandler(object sender, DbViewRowChangeEvent e);

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public delegate void DevCycleRowChangeEventHandler(object sender, DevCycleRowChangeEvent e);

        private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
        private DataRelation _relationComponentComponentData;
        private DataRelation _relationDbTableDbColumn;
        private DataRelation _relationDbTableDbView;
        private ComponentDataTable _tableComponent;
        private ComponentDataDataTable _tableComponentData;
        private DbColumnDataTable _tableDbColumn;
        private DbConstraintDataTable _tableDbConstraint;
        private DbFKeyDataTable _tableDbFKey;
        private DbGroupDataTable _tableDbGroup;
        private DbIndexDataTable _tableDbIndex;
        private DbTableDataTable _tableDbTable;
        private DbViewDataTable _tableDbView;
        private DevCycleDataTable _tableDevCycle;

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DatabaseSchema()
        {
            BeginInit();
            InitClass();
            var changeEventHandler = new CollectionChangeEventHandler(SchemaChanged);
            base.Tables.CollectionChanged += changeEventHandler;
            base.Relations.CollectionChanged += changeEventHandler;
            EndInit();
            foreach (DataTable dataTable in base.Tables)
            {
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    if (dataColumn.DataType == typeof (DateTime) &&
                        dataColumn.DateTimeMode != DataSetDateTime.Unspecified)
                        dataColumn.DateTimeMode = DataSetDateTime.Unspecified;
                }
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        protected DatabaseSchema(SerializationInfo info, StreamingContext context)
            : base(info, context, false)
        {
            if (IsBinarySerialized(info, context))
            {
                InitVars(false);
                var changeEventHandler = new CollectionChangeEventHandler(SchemaChanged);
                Tables.CollectionChanged += changeEventHandler;
                Relations.CollectionChanged += changeEventHandler;
            }
            else
            {
                var s = (string) info.GetValue("XmlSchema", typeof (string));
                if (DetermineSchemaSerializationMode(info, context) == SchemaSerializationMode.IncludeSchema)
                {
                    var dataSet = new DataSet();
                    dataSet.ReadXmlSchema(new XmlTextReader(new StringReader(s)));
                    if (dataSet.Tables["DevCycle"] != null)
                        base.Tables.Add(new DevCycleDataTable(dataSet.Tables["DevCycle"]));
                    if (dataSet.Tables["Component"] != null)
                        base.Tables.Add(new ComponentDataTable(dataSet.Tables["Component"]));
                    if (dataSet.Tables["DbGroup"] != null)
                        base.Tables.Add(new DbGroupDataTable(dataSet.Tables["DbGroup"]));
                    if (dataSet.Tables["ComponentData"] != null)
                        base.Tables.Add(new ComponentDataDataTable(dataSet.Tables["ComponentData"]));
                    if (dataSet.Tables["DbTable"] != null)
                        base.Tables.Add(new DbTableDataTable(dataSet.Tables["DbTable"]));
                    if (dataSet.Tables["DbColumn"] != null)
                        base.Tables.Add(new DbColumnDataTable(dataSet.Tables["DbColumn"]));
                    if (dataSet.Tables["DbConstraint"] != null)
                        base.Tables.Add(new DbConstraintDataTable(dataSet.Tables["DbConstraint"]));
                    if (dataSet.Tables["DbFKey"] != null)
                        base.Tables.Add(new DbFKeyDataTable(dataSet.Tables["DbFKey"]));
                    if (dataSet.Tables["DbIndex"] != null)
                        base.Tables.Add(new DbIndexDataTable(dataSet.Tables["DbIndex"]));
                    if (dataSet.Tables["DbView"] != null)
                        base.Tables.Add(new DbViewDataTable(dataSet.Tables["DbView"]));
                    DataSetName = dataSet.DataSetName;
                    Prefix = dataSet.Prefix;
                    Namespace = dataSet.Namespace;
                    Locale = dataSet.Locale;
                    CaseSensitive = dataSet.CaseSensitive;
                    EnforceConstraints = dataSet.EnforceConstraints;
                    Merge(dataSet, false, MissingSchemaAction.Add);
                    InitVars();
                }
                else
                    ReadXmlSchema(new XmlTextReader(new StringReader(s)));
                GetSerializationData(info, context);
                var changeEventHandler = new CollectionChangeEventHandler(SchemaChanged);
                base.Tables.CollectionChanged += changeEventHandler;
                Relations.CollectionChanged += changeEventHandler;
            }
        }

        [Browsable(false)]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DebuggerNonUserCode]
        public DevCycleDataTable DevCycle
        {
            get { return _tableDevCycle; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [Browsable(false)]
        public ComponentDataTable Component
        {
            get { return _tableComponent; }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DbGroupDataTable DbGroup
        {
            get { return _tableDbGroup; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public ComponentDataDataTable ComponentData
        {
            get { return _tableComponentData; }
        }

        [DebuggerNonUserCode]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DbTableDataTable DbTable
        {
            get { return _tableDbTable; }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DbColumnDataTable DbColumn
        {
            get { return _tableDbColumn; }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DbConstraintDataTable DbConstraint
        {
            get { return _tableDbConstraint; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        public DbFKeyDataTable DbFKey
        {
            get { return _tableDbFKey; }
        }

        [Browsable(false)]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DbIndexDataTable DbIndex
        {
            get { return _tableDbIndex; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public DbViewDataTable DbView
        {
            get { return _tableDbView; }
        }

        [Browsable(true)]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override SchemaSerializationMode SchemaSerializationMode
        {
            get { return _schemaSerializationMode; }
            set { _schemaSerializationMode = value; }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataTableCollection Tables
        {
            get { return base.Tables; }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DebuggerNonUserCode]
        public new DataRelationCollection Relations
        {
            get { return base.Relations; }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected override void InitializeDerivedDataSet()
        {
            BeginInit();
            InitClass();
            EndInit();
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public override DataSet Clone()
        {
            var databaseSchema = (DatabaseSchema) base.Clone();
            databaseSchema.InitVars();
            databaseSchema.SchemaSerializationMode = SchemaSerializationMode;
            return databaseSchema;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected override bool ShouldSerializeTables()
        {
            return false;
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        protected override bool ShouldSerializeRelations()
        {
            return false;
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        protected override void ReadXmlSerializable(XmlReader reader)
        {
            if (DetermineSchemaSerializationMode(reader) == SchemaSerializationMode.IncludeSchema)
            {
                Reset();
                var dataSet = new DataSet();
                var num = (int) dataSet.ReadXml(reader);
                if (dataSet.Tables["DevCycle"] != null)
                    base.Tables.Add(new DevCycleDataTable(dataSet.Tables["DevCycle"]));
                if (dataSet.Tables["Component"] != null)
                    base.Tables.Add(new ComponentDataTable(dataSet.Tables["Component"]));
                if (dataSet.Tables["DbGroup"] != null)
                    base.Tables.Add(new DbGroupDataTable(dataSet.Tables["DbGroup"]));
                if (dataSet.Tables["ComponentData"] != null)
                    base.Tables.Add(new ComponentDataDataTable(dataSet.Tables["ComponentData"]));
                if (dataSet.Tables["DbTable"] != null)
                    base.Tables.Add(new DbTableDataTable(dataSet.Tables["DbTable"]));
                if (dataSet.Tables["DbColumn"] != null)
                    base.Tables.Add(new DbColumnDataTable(dataSet.Tables["DbColumn"]));
                if (dataSet.Tables["DbConstraint"] != null)
                    base.Tables.Add(new DbConstraintDataTable(dataSet.Tables["DbConstraint"]));
                if (dataSet.Tables["DbFKey"] != null)
                    base.Tables.Add(new DbFKeyDataTable(dataSet.Tables["DbFKey"]));
                if (dataSet.Tables["DbIndex"] != null)
                    base.Tables.Add(new DbIndexDataTable(dataSet.Tables["DbIndex"]));
                if (dataSet.Tables["DbView"] != null)
                    base.Tables.Add(new DbViewDataTable(dataSet.Tables["DbView"]));
                DataSetName = dataSet.DataSetName;
                Prefix = dataSet.Prefix;
                Namespace = dataSet.Namespace;
                Locale = dataSet.Locale;
                CaseSensitive = dataSet.CaseSensitive;
                EnforceConstraints = dataSet.EnforceConstraints;
                Merge(dataSet, false, MissingSchemaAction.Add);
                InitVars();
            }
            else
            {
                var readXml = (int) ReadXml(reader);
                InitVars();
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        protected override XmlSchema GetSchemaSerializable()
        {
            var memoryStream = new MemoryStream();
            WriteXmlSchema(new XmlTextWriter(memoryStream, null));
            memoryStream.Position = 0L;
            return XmlSchema.Read(new XmlTextReader(memoryStream), null);
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        internal void InitVars()
        {
            InitVars(true);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        internal void InitVars(bool initTable)
        {
            _tableDevCycle = (DevCycleDataTable) base.Tables["DevCycle"];
            if (initTable && _tableDevCycle != null)
                _tableDevCycle.InitVars();
            _tableComponent = (ComponentDataTable) base.Tables["Component"];
            if (initTable && _tableComponent != null)
                _tableComponent.InitVars();
            _tableDbGroup = (DbGroupDataTable) base.Tables["DbGroup"];
            if (initTable && _tableDbGroup != null)
                _tableDbGroup.InitVars();
            _tableComponentData = (ComponentDataDataTable) base.Tables["ComponentData"];
            if (initTable && _tableComponentData != null)
                _tableComponentData.InitVars();
            _tableDbTable = (DbTableDataTable) base.Tables["DbTable"];
            if (initTable && _tableDbTable != null)
                _tableDbTable.InitVars();
            _tableDbColumn = (DbColumnDataTable) base.Tables["DbColumn"];
            if (initTable && _tableDbColumn != null)
                _tableDbColumn.InitVars();
            _tableDbConstraint = (DbConstraintDataTable) base.Tables["DbConstraint"];
            if (initTable && _tableDbConstraint != null)
                _tableDbConstraint.InitVars();
            _tableDbFKey = (DbFKeyDataTable) base.Tables["DbFKey"];
            if (initTable && _tableDbFKey != null)
                _tableDbFKey.InitVars();
            _tableDbIndex = (DbIndexDataTable) base.Tables["DbIndex"];
            if (initTable && _tableDbIndex != null)
                _tableDbIndex.InitVars();
            _tableDbView = (DbViewDataTable) base.Tables["DbView"];
            if (initTable && _tableDbView != null)
                _tableDbView.InitVars();
            _relationComponentComponentData = Relations["ComponentComponentData"];
            _relationDbTableDbColumn = Relations["DbTableDbColumn"];
            _relationDbTableDbView = Relations["DbTableDbView"];
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        private void InitClass()
        {
            DataSetName = "DatabaseSchema";
            Prefix = "";
            EnforceConstraints = true;
            SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
            _tableDevCycle = new DevCycleDataTable();
            base.Tables.Add(_tableDevCycle);
            _tableComponent = new ComponentDataTable();
            base.Tables.Add(_tableComponent);
            _tableDbGroup = new DbGroupDataTable();
            base.Tables.Add(_tableDbGroup);
            _tableComponentData = new ComponentDataDataTable();
            base.Tables.Add(_tableComponentData);
            _tableDbTable = new DbTableDataTable();
            base.Tables.Add(_tableDbTable);
            _tableDbColumn = new DbColumnDataTable();
            base.Tables.Add(_tableDbColumn);
            _tableDbConstraint = new DbConstraintDataTable();
            base.Tables.Add(_tableDbConstraint);
            _tableDbFKey = new DbFKeyDataTable();
            base.Tables.Add(_tableDbFKey);
            _tableDbIndex = new DbIndexDataTable();
            base.Tables.Add(_tableDbIndex);
            _tableDbView = new DbViewDataTable();
            base.Tables.Add(_tableDbView);
            var foreignKeyConstraint1 = new ForeignKeyConstraint("ComponentComponentData", new DataColumn[1]
            {
                _tableComponent.ComponentColumn
            }, new DataColumn[1]
            {
                _tableComponentData.ComponentColumn
            });
            _tableComponentData.Constraints.Add(foreignKeyConstraint1);
            foreignKeyConstraint1.AcceptRejectRule = AcceptRejectRule.None;
            foreignKeyConstraint1.DeleteRule = Rule.Cascade;
            foreignKeyConstraint1.UpdateRule = Rule.Cascade;
            var foreignKeyConstraint2 = new ForeignKeyConstraint("DbTableDbColumn", new DataColumn[1]
            {
                _tableDbTable.DbTableColumn
            }, new DataColumn[1]
            {
                _tableDbColumn.DbTableColumn
            });
            _tableDbColumn.Constraints.Add(foreignKeyConstraint2);
            foreignKeyConstraint2.AcceptRejectRule = AcceptRejectRule.None;
            foreignKeyConstraint2.DeleteRule = Rule.Cascade;
            foreignKeyConstraint2.UpdateRule = Rule.Cascade;
            var foreignKeyConstraint3 = new ForeignKeyConstraint("DbTableDbView", new[]
            {
                _tableDbTable.DbTableColumn
            }, new[]
            {
                _tableDbView.DbTableColumn
            });
            _tableDbView.Constraints.Add(foreignKeyConstraint3);
            foreignKeyConstraint3.AcceptRejectRule = AcceptRejectRule.None;
            foreignKeyConstraint3.DeleteRule = Rule.Cascade;
            foreignKeyConstraint3.UpdateRule = Rule.Cascade;
            _relationComponentComponentData = new DataRelation("ComponentComponentData", new[]
            {
                _tableComponent.ComponentColumn
            }, new[]
            {
                _tableComponentData.ComponentColumn
            }, false);
            Relations.Add(_relationComponentComponentData);
            _relationDbTableDbColumn = new DataRelation("DbTableDbColumn", new[]
            {
                _tableDbTable.DbTableColumn
            }, new[]
            {
                _tableDbColumn.DbTableColumn
            }, false);
            Relations.Add(_relationDbTableDbColumn);
            _relationDbTableDbView = new DataRelation("DbTableDbView", new[]
            {
                _tableDbTable.DbTableColumn
            }, new[]
            {
                _tableDbView.DbTableColumn
            }, false);
            Relations.Add(_relationDbTableDbView);
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        private bool ShouldSerializeDevCycle()
        {
            return false;
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        private bool ShouldSerializeComponent()
        {
            return false;
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        private bool ShouldSerializeDbGroup()
        {
            return false;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private bool ShouldSerializeComponentData()
        {
            return false;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private bool ShouldSerializeDbTable()
        {
            return false;
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        private bool ShouldSerializeDbColumn()
        {
            return false;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private bool ShouldSerializeDbConstraint()
        {
            return false;
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        private bool ShouldSerializeDbFKey()
        {
            return false;
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        private bool ShouldSerializeDbIndex()
        {
            return false;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private bool ShouldSerializeDbView()
        {
            return false;
        }

        [DebuggerNonUserCode]
        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        private void SchemaChanged(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action != CollectionChangeAction.Remove)
                return;
            InitVars();
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [DebuggerNonUserCode]
        public static XmlSchemaComplexType GetTypedDataSetSchema(XmlSchemaSet xs)
        {
            var databaseSchema = new DatabaseSchema();
            var schemaComplexType = new XmlSchemaComplexType();
            var xmlSchemaSequence = new XmlSchemaSequence();
            xmlSchemaSequence.Items.Add(new XmlSchemaAny
            {
                Namespace = databaseSchema.Namespace
            });
            schemaComplexType.Particle = xmlSchemaSequence;
            var schemaSerializable = databaseSchema.GetSchemaSerializable();
            if (schemaSerializable != null && xs.Contains(schemaSerializable.TargetNamespace))
            {
                var memoryStream1 = new MemoryStream();
                var memoryStream2 = new MemoryStream();
                try
                {
                    schemaSerializable.Write(memoryStream1);
                    foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                    {
                        memoryStream2.SetLength(0L);
                        xmlSchema.Write(memoryStream2);
                        if (memoryStream1.Length == memoryStream2.Length)
                        {
                            memoryStream1.Position = 0L;
                            memoryStream2.Position = 0L;
                            do
                            {
                            } while (memoryStream1.Position != memoryStream1.Length &&
                                     memoryStream1.ReadByte() == memoryStream2.ReadByte());
                            if (memoryStream1.Position == memoryStream1.Length)
                                return schemaComplexType;
                        }
                    }
                }
                finally
                {
                    memoryStream1.Close();
                    memoryStream2.Close();
                }
            }
            if (schemaSerializable != null) xs.Add(schemaSerializable);
            return schemaComplexType;
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class ComponentDataDataTable : TypedTableBase<ComponentDataRow>
        {
            private DataColumn _columnComponent;
            private DataColumn _columnDbGroup;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public ComponentDataDataTable()
            {
                TableName = "ComponentData";
                BeginInit();
                InitClass();
                EndInit();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal ComponentDataDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected ComponentDataDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn ComponentColumn
            {
                get { return _columnComponent; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbGroupColumn
            {
                get { return _columnDbGroup; }
            }

            [Browsable(false)]
            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int Count
            {
                get { return Rows.Count; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentDataRow this[int index]
            {
                get { return (ComponentDataRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentDataRowChangeEventHandler ComponentDataRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentDataRowChangeEventHandler ComponentDataRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentDataRowChangeEventHandler ComponentDataRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentDataRowChangeEventHandler ComponentDataRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddComponentDataRow(ComponentDataRow row)
            {
                Rows.Add(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public ComponentDataRow AddComponentDataRow(ComponentRow parentComponentRowByComponentComponentData,
                string dbGroup)
            {
                var componentDataRow = (ComponentDataRow) NewRow();
                var objArray = new object[]
                {
                    null,
                    dbGroup
                };
                if (parentComponentRowByComponentComponentData != null)
                    objArray[0] = parentComponentRowByComponentComponentData[0];
                componentDataRow.ItemArray = objArray;
                Rows.Add(componentDataRow);
                return componentDataRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public ComponentDataRow FindByComponentDbGroup(string Component, string DbGroup)
            {
                return (ComponentDataRow) Rows.Find(new object[2]
                {
                    Component,
                    DbGroup
                });
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public override DataTable Clone()
            {
                var componentDataDataTable = (ComponentDataDataTable) base.Clone();
                componentDataDataTable.InitVars();
                return componentDataDataTable;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataTable CreateInstance()
            {
                return new ComponentDataDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                _columnComponent = Columns["Component"];
                _columnDbGroup = Columns["DbGroup"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                _columnComponent = new DataColumn("Component", typeof (string), null, MappingType.Element);
                Columns.Add(_columnComponent);
                _columnDbGroup = new DataColumn("DbGroup", typeof (string), null, MappingType.Element);
                Columns.Add(_columnDbGroup);
                Constraints.Add(new UniqueConstraint("DbSchemaKey2", new DataColumn[2]
                {
                    _columnComponent,
                    _columnDbGroup
                }, 1 != 0));
                _columnComponent.AllowDBNull = false;
                _columnDbGroup.AllowDBNull = false;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentDataRow NewComponentDataRow()
            {
                return (ComponentDataRow) NewRow();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new ComponentDataRow(builder);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override Type GetRowType()
            {
                return typeof (ComponentDataRow);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (ComponentDataRowChanged == null)
                    return;
                ComponentDataRowChanged(this, new ComponentDataRowChangeEvent((ComponentDataRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (ComponentDataRowChanging == null)
                    return;
                ComponentDataRowChanging(this, new ComponentDataRowChangeEvent((ComponentDataRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (ComponentDataRowDeleted == null)
                    return;
                ComponentDataRowDeleted(this, new ComponentDataRowChangeEvent((ComponentDataRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (ComponentDataRowDeleting == null)
                    return;
                ComponentDataRowDeleting(this, new ComponentDataRowChangeEvent((ComponentDataRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void RemoveComponentDataRow(ComponentDataRow row)
            {
                Rows.Remove(row);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "ComponentDataDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        memoryStream1.Close();
                        memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class ComponentDataRow : DataRow
        {
            private readonly ComponentDataDataTable tableComponentData;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal ComponentDataRow(DataRowBuilder rb)
                : base(rb)
            {
                tableComponentData = (ComponentDataDataTable) Table;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string Component
            {
                get { return (string) this[tableComponentData.ComponentColumn]; }
                set { this[tableComponentData.ComponentColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbGroup
            {
                get { return (string) this[tableComponentData.DbGroupColumn]; }
                set { this[tableComponentData.DbGroupColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentRow ComponentRow
            {
                get { return (ComponentRow) GetParentRow(Table.ParentRelations["ComponentComponentData"]); }
                set { SetParentRow(value, Table.ParentRelations["ComponentComponentData"]); }
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class ComponentDataRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly ComponentDataRow eventRow;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentDataRowChangeEvent(ComponentDataRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentDataRow Row
            {
                get { return eventRow; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class ComponentDataTable : TypedTableBase<ComponentRow>
        {
            private DataColumn columnComponent;
            private DataColumn columnDescription;
            private DataColumn columnSortSeq;
            private DataColumn columnStatus;
            private DataColumn columnVersion;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public ComponentDataTable()
            {
                TableName = "Component";
                BeginInit();
                InitClass();
                EndInit();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal ComponentDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected ComponentDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn ComponentColumn
            {
                get { return columnComponent; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DescriptionColumn
            {
                get { return columnDescription; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn StatusColumn
            {
                get { return columnStatus; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn SortSeqColumn
            {
                get { return columnSortSeq; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn VersionColumn
            {
                get { return columnVersion; }
            }

            [DebuggerNonUserCode]
            [Browsable(false)]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int Count
            {
                get { return Rows.Count; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public ComponentRow this[int index]
            {
                get { return (ComponentRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentRowChangeEventHandler ComponentRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentRowChangeEventHandler ComponentRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentRowChangeEventHandler ComponentRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event ComponentRowChangeEventHandler ComponentRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddComponentRow(ComponentRow row)
            {
                Rows.Add(row);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentRow AddComponentRow(string Component, string Description, string Status, int SortSeq,
                string Version)
            {
                var componentRow = (ComponentRow) NewRow();
                var objArray = new object[5]
                {
                    Component,
                    Description,
                    Status,
                    SortSeq,
                    Version
                };
                componentRow.ItemArray = objArray;
                Rows.Add(componentRow);
                return componentRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public ComponentRow FindByComponent(string Component)
            {
                return (ComponentRow) Rows.Find(new object[1]
                {
                    Component
                });
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public override DataTable Clone()
            {
                var componentDataTable = (ComponentDataTable) base.Clone();
                componentDataTable.InitVars();
                return componentDataTable;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override DataTable CreateInstance()
            {
                return new ComponentDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnComponent = Columns["Component"];
                columnDescription = Columns["Description"];
                columnStatus = Columns["Status"];
                columnSortSeq = Columns["SortSeq"];
                columnVersion = Columns["Version"];
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private void InitClass()
            {
                columnComponent = new DataColumn("Component", typeof (string), null, MappingType.Element);
                Columns.Add(columnComponent);
                columnDescription = new DataColumn("Description", typeof (string), null, MappingType.Element);
                Columns.Add(columnDescription);
                columnStatus = new DataColumn("Status", typeof (string), null, MappingType.Element);
                Columns.Add(columnStatus);
                columnSortSeq = new DataColumn("SortSeq", typeof (int), null, MappingType.Element);
                Columns.Add(columnSortSeq);
                columnVersion = new DataColumn("Version", typeof (string), null, MappingType.Element);
                Columns.Add(columnVersion);
                Constraints.Add(new UniqueConstraint("DbSchemaKey1", new DataColumn[1]
                {
                    columnComponent
                }, 1 != 0));
                columnComponent.AllowDBNull = false;
                columnComponent.Unique = true;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public ComponentRow NewComponentRow()
            {
                return (ComponentRow) NewRow();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new ComponentRow(builder);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override Type GetRowType()
            {
                return typeof (ComponentRow);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (ComponentRowChanged == null)
                    return;
                ComponentRowChanged(this, new ComponentRowChangeEvent((ComponentRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (ComponentRowChanging == null)
                    return;
                ComponentRowChanging(this, new ComponentRowChangeEvent((ComponentRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (ComponentRowDeleted == null)
                    return;
                ComponentRowDeleted(this, new ComponentRowChangeEvent((ComponentRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (ComponentRowDeleting == null)
                    return;
                ComponentRowDeleting(this, new ComponentRowChangeEvent((ComponentRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void RemoveComponentRow(ComponentRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "ComponentDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        if (memoryStream1 != null)
                            memoryStream1.Close();
                        if (memoryStream2 != null)
                            memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class ComponentRow : DataRow
        {
            private readonly ComponentDataTable tableComponent;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal ComponentRow(DataRowBuilder rb)
                : base(rb)
            {
                tableComponent = (ComponentDataTable) Table;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string Component
            {
                get { return (string) this[tableComponent.ComponentColumn]; }
                set { this[tableComponent.ComponentColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string Description
            {
                get
                {
                    if (IsDescriptionNull())
                        return string.Empty;
                    return (string) this[tableComponent.DescriptionColumn];
                }
                set { this[tableComponent.DescriptionColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string Status
            {
                get
                {
                    if (IsStatusNull())
                        return string.Empty;
                    return (string) this[tableComponent.StatusColumn];
                }
                set { this[tableComponent.StatusColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int SortSeq
            {
                get
                {
                    if (IsSortSeqNull())
                        return 0;
                    return (int) this[tableComponent.SortSeqColumn];
                }
                set { this[tableComponent.SortSeqColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string Version
            {
                get
                {
                    if (IsVersionNull())
                        return "8.0";
                    return (string) this[tableComponent.VersionColumn];
                }
                set { this[tableComponent.VersionColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDescriptionNull()
            {
                return IsNull(tableComponent.DescriptionColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDescriptionNull()
            {
                this[tableComponent.DescriptionColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsStatusNull()
            {
                return IsNull(tableComponent.StatusColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetStatusNull()
            {
                this[tableComponent.StatusColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsSortSeqNull()
            {
                return IsNull(tableComponent.SortSeqColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetSortSeqNull()
            {
                this[tableComponent.SortSeqColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsVersionNull()
            {
                return IsNull(tableComponent.VersionColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetVersionNull()
            {
                this[tableComponent.VersionColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentDataRow[] GetComponentDataRows()
            {
                if (Table.ChildRelations["ComponentComponentData"] == null)
                    return new ComponentDataRow[0];
                return (ComponentDataRow[]) GetChildRows(Table.ChildRelations["ComponentComponentData"]);
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class ComponentRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly ComponentRow eventRow;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentRowChangeEvent(ComponentRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public ComponentRow Row
            {
                get { return eventRow; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DbColumnDataTable : TypedTableBase<DbColumnRow>
        {
            private DataColumn columnDbColumn;
            private DataColumn columnDbConstraint;
            private DataColumn columnDbDefault;
            private DataColumn columnDbDefinition;
            private DataColumn columnDbIndex;
            private DataColumn columnDbLen;
            private DataColumn columnDbNull;
            private DataColumn columnDbPrecision;
            private DataColumn columnDbPrevious;
            private DataColumn columnDbPrimaryKey;
            private DataColumn columnDbSeq;
            private DataColumn columnDbSysGen;
            private DataColumn columnDbTable;
            private DataColumn columnDbType;
            private DataColumn columnLabel;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbColumnDataTable()
            {
                TableName = "DbColumn";
                BeginInit();
                InitClass();
                EndInit();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbColumnDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected DbColumnDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbTableColumn
            {
                get { return columnDbTable; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbSeqColumn
            {
                get { return columnDbSeq; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbColumnColumn
            {
                get { return columnDbColumn; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbTypeColumn
            {
                get { return columnDbType; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbLenColumn
            {
                get { return columnDbLen; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbPrecisionColumn
            {
                get { return columnDbPrecision; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbDefinitionColumn
            {
                get { return columnDbDefinition; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbPrimaryKeyColumn
            {
                get { return columnDbPrimaryKey; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbNullColumn
            {
                get { return columnDbNull; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbIndexColumn
            {
                get { return columnDbIndex; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbSysGenColumn
            {
                get { return columnDbSysGen; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbDefaultColumn
            {
                get { return columnDbDefault; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbConstraintColumn
            {
                get { return columnDbConstraint; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbPreviousColumn
            {
                get { return columnDbPrevious; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn LabelColumn
            {
                get { return columnLabel; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [Browsable(false)]
            [DebuggerNonUserCode]
            public int Count
            {
                get { return Rows.Count; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbColumnRow this[int index]
            {
                get { return (DbColumnRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbColumnRowChangeEventHandler DbColumnRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbColumnRowChangeEventHandler DbColumnRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbColumnRowChangeEventHandler DbColumnRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbColumnRowChangeEventHandler DbColumnRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddDbColumnRow(DbColumnRow row)
            {
                Rows.Add(row);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbColumnRow AddDbColumnRow(DbTableRow parentDbTableRowByDbTableDbColumn, int DbSeq, string DbColumn,
                string DbType, int DbLen, int DbPrecision, string DbDefinition, int DbPrimaryKey, int DbNull,
                int DbIndex, int DbSysGen, string DbDefault, string DbConstraint, string DbPrevious, string Label)
            {
                var dbColumnRow = (DbColumnRow) NewRow();
                var objArray = new object[15]
                {
                    null,
                    DbSeq,
                    DbColumn,
                    DbType,
                    DbLen,
                    DbPrecision,
                    DbDefinition,
                    DbPrimaryKey,
                    DbNull,
                    DbIndex,
                    DbSysGen,
                    DbDefault,
                    DbConstraint,
                    DbPrevious,
                    Label
                };
                if (parentDbTableRowByDbTableDbColumn != null)
                    objArray[0] = parentDbTableRowByDbTableDbColumn[0];
                dbColumnRow.ItemArray = objArray;
                Rows.Add(dbColumnRow);
                return dbColumnRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbColumnRow FindByDbTableDbColumn(string DbTable, string DbColumn)
            {
                return (DbColumnRow) Rows.Find(new object[2]
                {
                    DbTable,
                    DbColumn
                });
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public override DataTable Clone()
            {
                var dbColumnDataTable = (DbColumnDataTable) base.Clone();
                dbColumnDataTable.InitVars();
                return dbColumnDataTable;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override DataTable CreateInstance()
            {
                return new DbColumnDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnDbTable = Columns["DbTable"];
                columnDbSeq = Columns["DbSeq"];
                columnDbColumn = Columns["DbColumn"];
                columnDbType = Columns["DbType"];
                columnDbLen = Columns["DbLen"];
                columnDbPrecision = Columns["DbPrecision"];
                columnDbDefinition = Columns["DbDefinition"];
                columnDbPrimaryKey = Columns["DbPrimaryKey"];
                columnDbNull = Columns["DbNull"];
                columnDbIndex = Columns["DbIndex"];
                columnDbSysGen = Columns["DbSysGen"];
                columnDbDefault = Columns["DbDefault"];
                columnDbConstraint = Columns["DbConstraint"];
                columnDbPrevious = Columns["DbPrevious"];
                columnLabel = Columns["Label"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                columnDbTable = new DataColumn("DbTable", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbTable);
                columnDbSeq = new DataColumn("DbSeq", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbSeq);
                columnDbColumn = new DataColumn("DbColumn", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbColumn);
                columnDbType = new DataColumn("DbType", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbType);
                columnDbLen = new DataColumn("DbLen", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbLen);
                columnDbPrecision = new DataColumn("DbPrecision", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbPrecision);
                columnDbDefinition = new DataColumn("DbDefinition", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbDefinition);
                columnDbPrimaryKey = new DataColumn("DbPrimaryKey", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbPrimaryKey);
                columnDbNull = new DataColumn("DbNull", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbNull);
                columnDbIndex = new DataColumn("DbIndex", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbIndex);
                columnDbSysGen = new DataColumn("DbSysGen", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbSysGen);
                columnDbDefault = new DataColumn("DbDefault", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbDefault);
                columnDbConstraint = new DataColumn("DbConstraint", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbConstraint);
                columnDbPrevious = new DataColumn("DbPrevious", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbPrevious);
                columnLabel = new DataColumn("Label", typeof (string), null, MappingType.Element);
                Columns.Add(columnLabel);
                Constraints.Add(new UniqueConstraint("DbColumnPKey", new DataColumn[2]
                {
                    columnDbTable,
                    columnDbColumn
                }, 1 != 0));
                columnDbTable.AllowDBNull = false;
                columnDbColumn.AllowDBNull = false;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbColumnRow NewDbColumnRow()
            {
                return (DbColumnRow) NewRow();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DbColumnRow(builder);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override Type GetRowType()
            {
                return typeof (DbColumnRow);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DbColumnRowChanged == null)
                    return;
                DbColumnRowChanged(this, new DbColumnRowChangeEvent((DbColumnRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DbColumnRowChanging == null)
                    return;
                DbColumnRowChanging(this, new DbColumnRowChangeEvent((DbColumnRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DbColumnRowDeleted == null)
                    return;
                DbColumnRowDeleted(this, new DbColumnRowChangeEvent((DbColumnRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DbColumnRowDeleting == null)
                    return;
                DbColumnRowDeleting(this, new DbColumnRowChangeEvent((DbColumnRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void RemoveDbColumnRow(DbColumnRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DbColumnDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        memoryStream1.Close();
                        memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DbColumnRow : DataRow
        {
            private readonly DbColumnDataTable tableDbColumn;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal DbColumnRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDbColumn = (DbColumnDataTable) Table;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbTable
            {
                get { return (string) this[tableDbColumn.DbTableColumn]; }
                set { this[tableDbColumn.DbTableColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbSeq
            {
                get
                {
                    if (IsDbSeqNull())
                        return 0;
                    return (int) this[tableDbColumn.DbSeqColumn];
                }
                set { this[tableDbColumn.DbSeqColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbColumn
            {
                get { return (string) this[tableDbColumn.DbColumnColumn]; }
                set { this[tableDbColumn.DbColumnColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbType
            {
                get
                {
                    if (IsDbTypeNull())
                        return string.Empty;
                    return (string) this[tableDbColumn.DbTypeColumn];
                }
                set { this[tableDbColumn.DbTypeColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbLen
            {
                get
                {
                    if (IsDbLenNull())
                        return 0;
                    return (int) this[tableDbColumn.DbLenColumn];
                }
                set { this[tableDbColumn.DbLenColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbPrecision
            {
                get
                {
                    if (IsDbPrecisionNull())
                        return 0;
                    return (int) this[tableDbColumn.DbPrecisionColumn];
                }
                set { this[tableDbColumn.DbPrecisionColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbDefinition
            {
                get
                {
                    if (IsDbDefinitionNull())
                        return string.Empty;
                    return (string) this[tableDbColumn.DbDefinitionColumn];
                }
                set { this[tableDbColumn.DbDefinitionColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbPrimaryKey
            {
                get
                {
                    if (IsDbPrimaryKeyNull())
                        return 0;
                    return (int) this[tableDbColumn.DbPrimaryKeyColumn];
                }
                set { this[tableDbColumn.DbPrimaryKeyColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbNull
            {
                get
                {
                    if (IsDbNullNull())
                        return 1;
                    return (int) this[tableDbColumn.DbNullColumn];
                }
                set { this[tableDbColumn.DbNullColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbIndex
            {
                get
                {
                    if (IsDbIndexNull())
                        return 0;
                    return (int) this[tableDbColumn.DbIndexColumn];
                }
                set { this[tableDbColumn.DbIndexColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbSysGen
            {
                get
                {
                    if (IsDbSysGenNull())
                        return 0;
                    return (int) this[tableDbColumn.DbSysGenColumn];
                }
                set { this[tableDbColumn.DbSysGenColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbDefault
            {
                get
                {
                    if (IsDbDefaultNull())
                        return string.Empty;
                    return (string) this[tableDbColumn.DbDefaultColumn];
                }
                set { this[tableDbColumn.DbDefaultColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbConstraint
            {
                get
                {
                    if (IsDbConstraintNull())
                        return string.Empty;
                    return (string) this[tableDbColumn.DbConstraintColumn];
                }
                set { this[tableDbColumn.DbConstraintColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbPrevious
            {
                get
                {
                    if (IsDbPreviousNull())
                        return string.Empty;
                    return (string) this[tableDbColumn.DbPreviousColumn];
                }
                set { this[tableDbColumn.DbPreviousColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string Label
            {
                get
                {
                    if (IsLabelNull())
                        return string.Empty;
                    return (string) this[tableDbColumn.LabelColumn];
                }
                set { this[tableDbColumn.LabelColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbTableRow DbTableRow
            {
                get { return (DbTableRow) GetParentRow(Table.ParentRelations["DbTableDbColumn"]); }
                set { SetParentRow(value, Table.ParentRelations["DbTableDbColumn"]); }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbSeqNull()
            {
                return IsNull(tableDbColumn.DbSeqColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbSeqNull()
            {
                this[tableDbColumn.DbSeqColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbTypeNull()
            {
                return IsNull(tableDbColumn.DbTypeColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbTypeNull()
            {
                this[tableDbColumn.DbTypeColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbLenNull()
            {
                return IsNull(tableDbColumn.DbLenColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbLenNull()
            {
                this[tableDbColumn.DbLenColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbPrecisionNull()
            {
                return IsNull(tableDbColumn.DbPrecisionColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbPrecisionNull()
            {
                this[tableDbColumn.DbPrecisionColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbDefinitionNull()
            {
                return IsNull(tableDbColumn.DbDefinitionColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbDefinitionNull()
            {
                this[tableDbColumn.DbDefinitionColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbPrimaryKeyNull()
            {
                return IsNull(tableDbColumn.DbPrimaryKeyColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbPrimaryKeyNull()
            {
                this[tableDbColumn.DbPrimaryKeyColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbNullNull()
            {
                return IsNull(tableDbColumn.DbNullColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbNullNull()
            {
                this[tableDbColumn.DbNullColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbIndexNull()
            {
                return IsNull(tableDbColumn.DbIndexColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbIndexNull()
            {
                this[tableDbColumn.DbIndexColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbSysGenNull()
            {
                return IsNull(tableDbColumn.DbSysGenColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbSysGenNull()
            {
                this[tableDbColumn.DbSysGenColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbDefaultNull()
            {
                return IsNull(tableDbColumn.DbDefaultColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbDefaultNull()
            {
                this[tableDbColumn.DbDefaultColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbConstraintNull()
            {
                return IsNull(tableDbColumn.DbConstraintColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbConstraintNull()
            {
                this[tableDbColumn.DbConstraintColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbPreviousNull()
            {
                return IsNull(tableDbColumn.DbPreviousColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbPreviousNull()
            {
                this[tableDbColumn.DbPreviousColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsLabelNull()
            {
                return IsNull(tableDbColumn.LabelColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetLabelNull()
            {
                this[tableDbColumn.LabelColumn] = Convert.DBNull;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DbColumnRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DbColumnRow eventRow;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbColumnRowChangeEvent(DbColumnRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbColumnRow Row
            {
                get { return eventRow; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DbConstraintDataTable : TypedTableBase<DbConstraintRow>
        {
            private DataColumn columnConstraintType;
            private DataColumn columnDbColumn;
            private DataColumn columnDbConstraint;
            private DataColumn columnDbExpr;
            private DataColumn columnDbSearch;
            private DataColumn columnDbTable;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbConstraintDataTable()
            {
                TableName = "DbConstraint";
                BeginInit();
                InitClass();
                EndInit();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal DbConstraintDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected DbConstraintDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbConstraintColumn
            {
                get { return columnDbConstraint; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn ConstraintTypeColumn
            {
                get { return columnConstraintType; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbTableColumn
            {
                get { return columnDbTable; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbColumnColumn
            {
                get { return columnDbColumn; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbSearchColumn
            {
                get { return columnDbSearch; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbExprColumn
            {
                get { return columnDbExpr; }
            }

            [DebuggerNonUserCode]
            [Browsable(false)]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int Count
            {
                get { return Rows.Count; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbConstraintRow this[int index]
            {
                get { return (DbConstraintRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbConstraintRowChangeEventHandler DbConstraintRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbConstraintRowChangeEventHandler DbConstraintRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbConstraintRowChangeEventHandler DbConstraintRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbConstraintRowChangeEventHandler DbConstraintRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddDbConstraintRow(DbConstraintRow row)
            {
                Rows.Add(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbConstraintRow AddDbConstraintRow(string DbConstraint, string ConstraintType, string DbTable,
                string DbColumn, string DbSearch, string DbExpr)
            {
                var dbConstraintRow = (DbConstraintRow) NewRow();
                var objArray = new object[6]
                {
                    DbConstraint,
                    ConstraintType,
                    DbTable,
                    DbColumn,
                    DbSearch,
                    DbExpr
                };
                dbConstraintRow.ItemArray = objArray;
                Rows.Add(dbConstraintRow);
                return dbConstraintRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbConstraintRow FindByDbConstraint(string DbConstraint)
            {
                return (DbConstraintRow) Rows.Find(new object[1]
                {
                    DbConstraint
                });
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public override DataTable Clone()
            {
                var constraintDataTable = (DbConstraintDataTable) base.Clone();
                constraintDataTable.InitVars();
                return constraintDataTable;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataTable CreateInstance()
            {
                return new DbConstraintDataTable();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal void InitVars()
            {
                columnDbConstraint = Columns["DbConstraint"];
                columnConstraintType = Columns["ConstraintType"];
                columnDbTable = Columns["DbTable"];
                columnDbColumn = Columns["DbColumn"];
                columnDbSearch = Columns["DbSearch"];
                columnDbExpr = Columns["DbExpr"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                columnDbConstraint = new DataColumn("DbConstraint", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbConstraint);
                columnConstraintType = new DataColumn("ConstraintType", typeof (string), null, MappingType.Element);
                Columns.Add(columnConstraintType);
                columnDbTable = new DataColumn("DbTable", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbTable);
                columnDbColumn = new DataColumn("DbColumn", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbColumn);
                columnDbSearch = new DataColumn("DbSearch", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbSearch);
                columnDbExpr = new DataColumn("DbExpr", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbExpr);
                Constraints.Add(new UniqueConstraint("DbConstraintPKey", new DataColumn[1]
                {
                    columnDbConstraint
                }, 1 != 0));
                columnDbConstraint.AllowDBNull = false;
                columnDbConstraint.Unique = true;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbConstraintRow NewDbConstraintRow()
            {
                return (DbConstraintRow) NewRow();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DbConstraintRow(builder);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override Type GetRowType()
            {
                return typeof (DbConstraintRow);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DbConstraintRowChanged == null)
                    return;
                DbConstraintRowChanged(this, new DbConstraintRowChangeEvent((DbConstraintRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DbConstraintRowChanging == null)
                    return;
                DbConstraintRowChanging(this, new DbConstraintRowChangeEvent((DbConstraintRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DbConstraintRowDeleted == null)
                    return;
                DbConstraintRowDeleted(this, new DbConstraintRowChangeEvent((DbConstraintRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DbConstraintRowDeleting == null)
                    return;
                DbConstraintRowDeleting(this, new DbConstraintRowChangeEvent((DbConstraintRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void RemoveDbConstraintRow(DbConstraintRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DbConstraintDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        if (memoryStream1 != null)
                            memoryStream1.Close();
                        if (memoryStream2 != null)
                            memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DbConstraintRow : DataRow
        {
            private readonly DbConstraintDataTable tableDbConstraint;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbConstraintRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDbConstraint = (DbConstraintDataTable) Table;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbConstraint
            {
                get { return (string) this[tableDbConstraint.DbConstraintColumn]; }
                set { this[tableDbConstraint.DbConstraintColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string ConstraintType
            {
                get
                {
                    if (IsConstraintTypeNull())
                        return string.Empty;
                    return (string) this[tableDbConstraint.ConstraintTypeColumn];
                }
                set { this[tableDbConstraint.ConstraintTypeColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbTable
            {
                get
                {
                    if (IsDbTableNull())
                        return string.Empty;
                    return (string) this[tableDbConstraint.DbTableColumn];
                }
                set { this[tableDbConstraint.DbTableColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbColumn
            {
                get
                {
                    if (IsDbColumnNull())
                        return string.Empty;
                    return (string) this[tableDbConstraint.DbColumnColumn];
                }
                set { this[tableDbConstraint.DbColumnColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbSearch
            {
                get
                {
                    if (IsDbSearchNull())
                        return string.Empty;
                    return (string) this[tableDbConstraint.DbSearchColumn];
                }
                set { this[tableDbConstraint.DbSearchColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbExpr
            {
                get
                {
                    if (IsDbExprNull())
                        return string.Empty;
                    return (string) this[tableDbConstraint.DbExprColumn];
                }
                set { this[tableDbConstraint.DbExprColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsConstraintTypeNull()
            {
                return IsNull(tableDbConstraint.ConstraintTypeColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetConstraintTypeNull()
            {
                this[tableDbConstraint.ConstraintTypeColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbTableNull()
            {
                return IsNull(tableDbConstraint.DbTableColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbTableNull()
            {
                this[tableDbConstraint.DbTableColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbColumnNull()
            {
                return IsNull(tableDbConstraint.DbColumnColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbColumnNull()
            {
                this[tableDbConstraint.DbColumnColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbSearchNull()
            {
                return IsNull(tableDbConstraint.DbSearchColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbSearchNull()
            {
                this[tableDbConstraint.DbSearchColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbExprNull()
            {
                return IsNull(tableDbConstraint.DbExprColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbExprNull()
            {
                this[tableDbConstraint.DbExprColumn] = Convert.DBNull;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DbConstraintRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DbConstraintRow eventRow;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbConstraintRowChangeEvent(DbConstraintRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbConstraintRow Row
            {
                get { return eventRow; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DbFKeyDataTable : TypedTableBase<DbFKeyRow>
        {
            private DataColumn columnDbFKey;
            private DataColumn columnDbFKeyColSeq;
            private DataColumn columnDbFKeyColumn;
            private DataColumn columnDbFKeyTable;
            private DataColumn columnDbRefTable;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbFKeyDataTable()
            {
                TableName = "DbFKey";
                BeginInit();
                InitClass();
                EndInit();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbFKeyDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected DbFKeyDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbFKeyColumn
            {
                get { return columnDbFKey; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbFKeyTableColumn
            {
                get { return columnDbFKeyTable; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbFKeyColSeqColumn
            {
                get { return columnDbFKeyColSeq; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbFKeyColumnColumn
            {
                get { return columnDbFKeyColumn; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbRefTableColumn
            {
                get { return columnDbRefTable; }
            }

            [DebuggerNonUserCode]
            [Browsable(false)]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int Count
            {
                get { return Rows.Count; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbFKeyRow this[int index]
            {
                get { return (DbFKeyRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbFKeyRowChangeEventHandler DbFKeyRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbFKeyRowChangeEventHandler DbFKeyRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbFKeyRowChangeEventHandler DbFKeyRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbFKeyRowChangeEventHandler DbFKeyRowDeleted;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void AddDbFKeyRow(DbFKeyRow row)
            {
                Rows.Add(row);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbFKeyRow AddDbFKeyRow(string DbFKey, string DbFKeyTable, int DbFKeyColSeq, string DbFKeyColumn,
                string DbRefTable)
            {
                var dbFkeyRow = (DbFKeyRow) NewRow();
                var objArray = new object[5]
                {
                    DbFKey,
                    DbFKeyTable,
                    DbFKeyColSeq,
                    DbFKeyColumn,
                    DbRefTable
                };
                dbFkeyRow.ItemArray = objArray;
                Rows.Add(dbFkeyRow);
                return dbFkeyRow;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public override DataTable Clone()
            {
                var dbFkeyDataTable = (DbFKeyDataTable) base.Clone();
                dbFkeyDataTable.InitVars();
                return dbFkeyDataTable;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataTable CreateInstance()
            {
                return new DbFKeyDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnDbFKey = Columns["DbFKey"];
                columnDbFKeyTable = Columns["DbFKeyTable"];
                columnDbFKeyColSeq = Columns["DbFKeyColSeq"];
                columnDbFKeyColumn = Columns["DbFKeyColumn"];
                columnDbRefTable = Columns["DbRefTable"];
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            private void InitClass()
            {
                columnDbFKey = new DataColumn("DbFKey", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbFKey);
                columnDbFKeyTable = new DataColumn("DbFKeyTable", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbFKeyTable);
                columnDbFKeyColSeq = new DataColumn("DbFKeyColSeq", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbFKeyColSeq);
                columnDbFKeyColumn = new DataColumn("DbFKeyColumn", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbFKeyColumn);
                columnDbRefTable = new DataColumn("DbRefTable", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbRefTable);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbFKeyRow NewDbFKeyRow()
            {
                return (DbFKeyRow) NewRow();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DbFKeyRow(builder);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override Type GetRowType()
            {
                return typeof (DbFKeyRow);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DbFKeyRowChanged == null)
                    return;
                DbFKeyRowChanged(this, new DbFKeyRowChangeEvent((DbFKeyRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DbFKeyRowChanging == null)
                    return;
                DbFKeyRowChanging(this, new DbFKeyRowChangeEvent((DbFKeyRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DbFKeyRowDeleted == null)
                    return;
                DbFKeyRowDeleted(this, new DbFKeyRowChangeEvent((DbFKeyRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DbFKeyRowDeleting == null)
                    return;
                DbFKeyRowDeleting(this, new DbFKeyRowChangeEvent((DbFKeyRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void RemoveDbFKeyRow(DbFKeyRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DbFKeyDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        if (memoryStream1 != null)
                            memoryStream1.Close();
                        if (memoryStream2 != null)
                            memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DbFKeyRow : DataRow
        {
            private readonly DbFKeyDataTable tableDbFKey;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbFKeyRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDbFKey = (DbFKeyDataTable) Table;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbFKey
            {
                get
                {
                    if (IsDbFKeyNull())
                        return string.Empty;
                    return (string) this[tableDbFKey.DbFKeyColumn];
                }
                set { this[tableDbFKey.DbFKeyColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbFKeyTable
            {
                get
                {
                    if (IsDbFKeyTableNull())
                        return string.Empty;
                    return (string) this[tableDbFKey.DbFKeyTableColumn];
                }
                set { this[tableDbFKey.DbFKeyTableColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int DbFKeyColSeq
            {
                get
                {
                    if (IsDbFKeyColSeqNull())
                        return 0;
                    return (int) this[tableDbFKey.DbFKeyColSeqColumn];
                }
                set { this[tableDbFKey.DbFKeyColSeqColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbFKeyColumn
            {
                get
                {
                    if (IsDbFKeyColumnNull())
                        return string.Empty;
                    return (string) this[tableDbFKey.DbFKeyColumnColumn];
                }
                set { this[tableDbFKey.DbFKeyColumnColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbRefTable
            {
                get
                {
                    if (IsDbRefTableNull())
                        return string.Empty;
                    return (string) this[tableDbFKey.DbRefTableColumn];
                }
                set { this[tableDbFKey.DbRefTableColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbFKeyNull()
            {
                return IsNull(tableDbFKey.DbFKeyColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbFKeyNull()
            {
                this[tableDbFKey.DbFKeyColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbFKeyTableNull()
            {
                return IsNull(tableDbFKey.DbFKeyTableColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbFKeyTableNull()
            {
                this[tableDbFKey.DbFKeyTableColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbFKeyColSeqNull()
            {
                return IsNull(tableDbFKey.DbFKeyColSeqColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbFKeyColSeqNull()
            {
                this[tableDbFKey.DbFKeyColSeqColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbFKeyColumnNull()
            {
                return IsNull(tableDbFKey.DbFKeyColumnColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbFKeyColumnNull()
            {
                this[tableDbFKey.DbFKeyColumnColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbRefTableNull()
            {
                return IsNull(tableDbFKey.DbRefTableColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbRefTableNull()
            {
                this[tableDbFKey.DbRefTableColumn] = Convert.DBNull;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DbFKeyRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DbFKeyRow eventRow;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbFKeyRowChangeEvent(DbFKeyRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbFKeyRow Row
            {
                get { return eventRow; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DbGroupDataTable : TypedTableBase<DbGroupRow>
        {
            private DataColumn columnConfiguration;
            private DataColumn columnConversion;
            private DataColumn columnDbGroup;
            private DataColumn columnSortSeq;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbGroupDataTable()
            {
                TableName = "DbGroup";
                BeginInit();
                InitClass();
                EndInit();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbGroupDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected DbGroupDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbGroupColumn
            {
                get { return columnDbGroup; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn ConversionColumn
            {
                get { return columnConversion; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn ConfigurationColumn
            {
                get { return columnConfiguration; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn SortSeqColumn
            {
                get { return columnSortSeq; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [Browsable(false)]
            public int Count
            {
                get { return Rows.Count; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbGroupRow this[int index]
            {
                get { return (DbGroupRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbGroupRowChangeEventHandler DbGroupRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbGroupRowChangeEventHandler DbGroupRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbGroupRowChangeEventHandler DbGroupRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbGroupRowChangeEventHandler DbGroupRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddDbGroupRow(DbGroupRow row)
            {
                Rows.Add(row);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbGroupRow AddDbGroupRow(string DbGroup, string Conversion, string Configuration, int SortSeq)
            {
                var dbGroupRow = (DbGroupRow) NewRow();
                var objArray = new object[4]
                {
                    DbGroup,
                    Conversion,
                    Configuration,
                    SortSeq
                };
                dbGroupRow.ItemArray = objArray;
                Rows.Add(dbGroupRow);
                return dbGroupRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbGroupRow FindByDbGroup(string DbGroup)
            {
                return (DbGroupRow) Rows.Find(new object[1]
                {
                    DbGroup
                });
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public override DataTable Clone()
            {
                var dbGroupDataTable = (DbGroupDataTable) base.Clone();
                dbGroupDataTable.InitVars();
                return dbGroupDataTable;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataTable CreateInstance()
            {
                return new DbGroupDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnDbGroup = Columns["DbGroup"];
                columnConversion = Columns["Conversion"];
                columnConfiguration = Columns["Configuration"];
                columnSortSeq = Columns["SortSeq"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                columnDbGroup = new DataColumn("DbGroup", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbGroup);
                columnConversion = new DataColumn("Conversion", typeof (string), null, MappingType.Element);
                Columns.Add(columnConversion);
                columnConfiguration = new DataColumn("Configuration", typeof (string), null, MappingType.Element);
                Columns.Add(columnConfiguration);
                columnSortSeq = new DataColumn("SortSeq", typeof (int), null, MappingType.Element);
                Columns.Add(columnSortSeq);
                Constraints.Add(new UniqueConstraint("DbSchemaKey3", new DataColumn[1]
                {
                    columnDbGroup
                }, 1 != 0));
                columnDbGroup.AllowDBNull = false;
                columnDbGroup.Unique = true;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbGroupRow NewDbGroupRow()
            {
                return (DbGroupRow) NewRow();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DbGroupRow(builder);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override Type GetRowType()
            {
                return typeof (DbGroupRow);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DbGroupRowChanged == null)
                    return;
                DbGroupRowChanged(this, new DbGroupRowChangeEvent((DbGroupRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DbGroupRowChanging == null)
                    return;
                DbGroupRowChanging(this, new DbGroupRowChangeEvent((DbGroupRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DbGroupRowDeleted == null)
                    return;
                DbGroupRowDeleted(this, new DbGroupRowChangeEvent((DbGroupRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DbGroupRowDeleting == null)
                    return;
                DbGroupRowDeleting(this, new DbGroupRowChangeEvent((DbGroupRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void RemoveDbGroupRow(DbGroupRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DbGroupDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        if (memoryStream1 != null)
                            memoryStream1.Close();
                        if (memoryStream2 != null)
                            memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DbGroupRow : DataRow
        {
            private readonly DbGroupDataTable tableDbGroup;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal DbGroupRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDbGroup = (DbGroupDataTable) Table;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbGroup
            {
                get { return (string) this[tableDbGroup.DbGroupColumn]; }
                set { this[tableDbGroup.DbGroupColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string Conversion
            {
                get
                {
                    if (IsConversionNull())
                        return string.Empty;
                    return (string) this[tableDbGroup.ConversionColumn];
                }
                set { this[tableDbGroup.ConversionColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string Configuration
            {
                get
                {
                    if (IsConfigurationNull())
                        return string.Empty;
                    return (string) this[tableDbGroup.ConfigurationColumn];
                }
                set { this[tableDbGroup.ConfigurationColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int SortSeq
            {
                get
                {
                    if (IsSortSeqNull())
                        return 0;
                    return (int) this[tableDbGroup.SortSeqColumn];
                }
                set { this[tableDbGroup.SortSeqColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsConversionNull()
            {
                return IsNull(tableDbGroup.ConversionColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetConversionNull()
            {
                this[tableDbGroup.ConversionColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsConfigurationNull()
            {
                return IsNull(tableDbGroup.ConfigurationColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetConfigurationNull()
            {
                this[tableDbGroup.ConfigurationColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsSortSeqNull()
            {
                return IsNull(tableDbGroup.SortSeqColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetSortSeqNull()
            {
                this[tableDbGroup.SortSeqColumn] = Convert.DBNull;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DbGroupRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DbGroupRow eventRow;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbGroupRowChangeEvent(DbGroupRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbGroupRow Row
            {
                get { return eventRow; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DbIndexDataTable : TypedTableBase<DbIndexRow>
        {
            private DataColumn columnDbIndex;
            private DataColumn columnDbIndexColSeq;
            private DataColumn columnDbIndexColumn;
            private DataColumn columnDbIndexTable;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbIndexDataTable()
            {
                TableName = "DbIndex";
                BeginInit();
                InitClass();
                EndInit();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal DbIndexDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected DbIndexDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbIndexColumn
            {
                get { return columnDbIndex; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbIndexTableColumn
            {
                get { return columnDbIndexTable; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbIndexColSeqColumn
            {
                get { return columnDbIndexColSeq; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbIndexColumnColumn
            {
                get { return columnDbIndexColumn; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [Browsable(false)]
            public int Count
            {
                get { return Rows.Count; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbIndexRow this[int index]
            {
                get { return (DbIndexRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbIndexRowChangeEventHandler DbIndexRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbIndexRowChangeEventHandler DbIndexRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbIndexRowChangeEventHandler DbIndexRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbIndexRowChangeEventHandler DbIndexRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddDbIndexRow(DbIndexRow row)
            {
                Rows.Add(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbIndexRow AddDbIndexRow(string DbIndex, string DbIndexTable, int DbIndexColSeq, string DbIndexColumn)
            {
                var dbIndexRow = (DbIndexRow) NewRow();
                var objArray = new object[4]
                {
                    DbIndex,
                    DbIndexTable,
                    DbIndexColSeq,
                    DbIndexColumn
                };
                dbIndexRow.ItemArray = objArray;
                Rows.Add(dbIndexRow);
                return dbIndexRow;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public override DataTable Clone()
            {
                var dbIndexDataTable = (DbIndexDataTable) base.Clone();
                dbIndexDataTable.InitVars();
                return dbIndexDataTable;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override DataTable CreateInstance()
            {
                return new DbIndexDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnDbIndex = Columns["DbIndex"];
                columnDbIndexTable = Columns["DbIndexTable"];
                columnDbIndexColSeq = Columns["DbIndexColSeq"];
                columnDbIndexColumn = Columns["DbIndexColumn"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                columnDbIndex = new DataColumn("DbIndex", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbIndex);
                columnDbIndexTable = new DataColumn("DbIndexTable", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbIndexTable);
                columnDbIndexColSeq = new DataColumn("DbIndexColSeq", typeof (int), null, MappingType.Element);
                Columns.Add(columnDbIndexColSeq);
                columnDbIndexColumn = new DataColumn("DbIndexColumn", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbIndexColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbIndexRow NewDbIndexRow()
            {
                return (DbIndexRow) NewRow();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DbIndexRow(builder);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override Type GetRowType()
            {
                return typeof (DbIndexRow);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DbIndexRowChanged == null)
                    return;
                DbIndexRowChanged(this, new DbIndexRowChangeEvent((DbIndexRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DbIndexRowChanging == null)
                    return;
                DbIndexRowChanging(this, new DbIndexRowChangeEvent((DbIndexRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DbIndexRowDeleted == null)
                    return;
                DbIndexRowDeleted(this, new DbIndexRowChangeEvent((DbIndexRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DbIndexRowDeleting == null)
                    return;
                DbIndexRowDeleting(this, new DbIndexRowChangeEvent((DbIndexRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void RemoveDbIndexRow(DbIndexRow row)
            {
                Rows.Remove(row);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DbIndexDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        if (memoryStream1 != null)
                            memoryStream1.Close();
                        if (memoryStream2 != null)
                            memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DbIndexRow : DataRow
        {
            private readonly DbIndexDataTable tableDbIndex;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbIndexRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDbIndex = (DbIndexDataTable) Table;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbIndex
            {
                get
                {
                    if (IsDbIndexNull())
                        return string.Empty;
                    return (string) this[tableDbIndex.DbIndexColumn];
                }
                set { this[tableDbIndex.DbIndexColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbIndexTable
            {
                get
                {
                    if (IsDbIndexTableNull())
                        return string.Empty;
                    return (string) this[tableDbIndex.DbIndexTableColumn];
                }
                set { this[tableDbIndex.DbIndexTableColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int DbIndexColSeq
            {
                get
                {
                    if (IsDbIndexColSeqNull())
                        return 0;
                    return (int) this[tableDbIndex.DbIndexColSeqColumn];
                }
                set { this[tableDbIndex.DbIndexColSeqColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbIndexColumn
            {
                get
                {
                    if (IsDbIndexColumnNull())
                        return string.Empty;
                    return (string) this[tableDbIndex.DbIndexColumnColumn];
                }
                set { this[tableDbIndex.DbIndexColumnColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbIndexNull()
            {
                return IsNull(tableDbIndex.DbIndexColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbIndexNull()
            {
                this[tableDbIndex.DbIndexColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbIndexTableNull()
            {
                return IsNull(tableDbIndex.DbIndexTableColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbIndexTableNull()
            {
                this[tableDbIndex.DbIndexTableColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbIndexColSeqNull()
            {
                return IsNull(tableDbIndex.DbIndexColSeqColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbIndexColSeqNull()
            {
                this[tableDbIndex.DbIndexColSeqColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDbIndexColumnNull()
            {
                return IsNull(tableDbIndex.DbIndexColumnColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbIndexColumnNull()
            {
                this[tableDbIndex.DbIndexColumnColumn] = Convert.DBNull;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DbIndexRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DbIndexRow eventRow;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbIndexRowChangeEvent(DbIndexRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbIndexRow Row
            {
                get { return eventRow; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DbTableDataTable : TypedTableBase<DbTableRow>
        {
            private DataColumn columnDataMethod;
            private DataColumn columnDbDefinition;
            private DataColumn columnDbGroup;
            private DataColumn columnDbPrevious;
            private DataColumn columnDbTable;
            private DataColumn columnLabel;
            private DataColumn columnScopeStatus;
            private DataColumn columnViewSQL;
            private DataColumn columnViewSeq;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbTableDataTable()
            {
                TableName = "DbTable";
                BeginInit();
                InitClass();
                EndInit();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbTableDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected DbTableDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbTableColumn
            {
                get { return columnDbTable; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbGroupColumn
            {
                get { return columnDbGroup; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbDefinitionColumn
            {
                get { return columnDbDefinition; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DbPreviousColumn
            {
                get { return columnDbPrevious; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn LabelColumn
            {
                get { return columnLabel; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DataMethodColumn
            {
                get { return columnDataMethod; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn ScopeStatusColumn
            {
                get { return columnScopeStatus; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn ViewSQLColumn
            {
                get { return columnViewSQL; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn ViewSeqColumn
            {
                get { return columnViewSeq; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [Browsable(false)]
            [DebuggerNonUserCode]
            public int Count
            {
                get { return Rows.Count; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbTableRow this[int index]
            {
                get { return (DbTableRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbTableRowChangeEventHandler DbTableRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbTableRowChangeEventHandler DbTableRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbTableRowChangeEventHandler DbTableRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbTableRowChangeEventHandler DbTableRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddDbTableRow(DbTableRow row)
            {
                Rows.Add(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbTableRow AddDbTableRow(string DbTable, string DbGroup, string DbDefinition, string DbPrevious,
                string Label, string DataMethod, bool ScopeStatus, string ViewSQL, int ViewSeq)
            {
                var dbTableRow = (DbTableRow) NewRow();
                var objArray = new object[9]
                {
                    DbTable,
                    DbGroup,
                    DbDefinition,
                    DbPrevious,
                    Label,
                    DataMethod,
                    ScopeStatus,
                    ViewSQL,
                    ViewSeq
                };
                dbTableRow.ItemArray = objArray;
                Rows.Add(dbTableRow);
                return dbTableRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbTableRow FindByDbTable(string DbTable)
            {
                return (DbTableRow) Rows.Find(new object[1]
                {
                    DbTable
                });
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public override DataTable Clone()
            {
                var dbTableDataTable = (DbTableDataTable) base.Clone();
                dbTableDataTable.InitVars();
                return dbTableDataTable;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataTable CreateInstance()
            {
                return new DbTableDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnDbTable = Columns["DbTable"];
                columnDbGroup = Columns["DbGroup"];
                columnDbDefinition = Columns["DbDefinition"];
                columnDbPrevious = Columns["DbPrevious"];
                columnLabel = Columns["Label"];
                columnDataMethod = Columns["DataMethod"];
                columnScopeStatus = Columns["ScopeStatus"];
                columnViewSQL = Columns["ViewSQL"];
                columnViewSeq = Columns["ViewSeq"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                columnDbTable = new DataColumn("DbTable", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbTable);
                columnDbGroup = new DataColumn("DbGroup", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbGroup);
                columnDbDefinition = new DataColumn("DbDefinition", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbDefinition);
                columnDbPrevious = new DataColumn("DbPrevious", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbPrevious);
                columnLabel = new DataColumn("Label", typeof (string), null, MappingType.Element);
                Columns.Add(columnLabel);
                columnDataMethod = new DataColumn("DataMethod", typeof (string), null, MappingType.Element);
                Columns.Add(columnDataMethod);
                columnScopeStatus = new DataColumn("ScopeStatus", typeof (bool), null, MappingType.Element);
                Columns.Add(columnScopeStatus);
                columnViewSQL = new DataColumn("ViewSQL", typeof (string), null, MappingType.Element);
                Columns.Add(columnViewSQL);
                columnViewSeq = new DataColumn("ViewSeq", typeof (int), null, MappingType.Element);
                Columns.Add(columnViewSeq);
                Constraints.Add(new UniqueConstraint("DbTablePKey", new DataColumn[1]
                {
                    columnDbTable
                }, 1 != 0));
                columnDbTable.AllowDBNull = false;
                columnDbTable.Unique = true;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbTableRow NewDbTableRow()
            {
                return (DbTableRow) NewRow();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DbTableRow(builder);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override Type GetRowType()
            {
                return typeof (DbTableRow);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DbTableRowChanged == null)
                    return;
                DbTableRowChanged(this, new DbTableRowChangeEvent((DbTableRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DbTableRowChanging == null)
                    return;
                DbTableRowChanging(this, new DbTableRowChangeEvent((DbTableRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DbTableRowDeleted == null)
                    return;
                DbTableRowDeleted(this, new DbTableRowChangeEvent((DbTableRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DbTableRowDeleting == null)
                    return;
                DbTableRowDeleting(this, new DbTableRowChangeEvent((DbTableRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void RemoveDbTableRow(DbTableRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DbTableDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        if (memoryStream1 != null)
                            memoryStream1.Close();
                        if (memoryStream2 != null)
                            memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DbTableRow : DataRow
        {
            private readonly DbTableDataTable tableDbTable;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal DbTableRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDbTable = (DbTableDataTable) Table;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbTable
            {
                get { return (string) this[tableDbTable.DbTableColumn]; }
                set { this[tableDbTable.DbTableColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbGroup
            {
                get
                {
                    if (IsDbGroupNull())
                        return string.Empty;
                    return (string) this[tableDbTable.DbGroupColumn];
                }
                set { this[tableDbTable.DbGroupColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbDefinition
            {
                get
                {
                    if (IsDbDefinitionNull())
                        return string.Empty;
                    return (string) this[tableDbTable.DbDefinitionColumn];
                }
                set { this[tableDbTable.DbDefinitionColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string DbPrevious
            {
                get
                {
                    if (IsDbPreviousNull())
                        return string.Empty;
                    return (string) this[tableDbTable.DbPreviousColumn];
                }
                set { this[tableDbTable.DbPreviousColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string Label
            {
                get
                {
                    if (IsLabelNull())
                        return string.Empty;
                    return (string) this[tableDbTable.LabelColumn];
                }
                set { this[tableDbTable.LabelColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DataMethod
            {
                get
                {
                    if (IsDataMethodNull())
                        return string.Empty;
                    return (string) this[tableDbTable.DataMethodColumn];
                }
                set { this[tableDbTable.DataMethodColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool ScopeStatus
            {
                get
                {
                    try
                    {
                        return (bool) this[tableDbTable.ScopeStatusColumn];
                    }
                    catch (InvalidCastException ex)
                    {
                        throw new StrongTypingException(
                            "The value for column 'ScopeStatus' in table 'DbTable' is DBNull.", ex);
                    }
                }
                set
                {
                    bool scopStatusval = value;
                    this[tableDbTable.ScopeStatusColumn] = scopStatusval;
                }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public string ViewSQL
            {
                get
                {
                    if (IsViewSQLNull())
                        return string.Empty;
                    return (string) this[tableDbTable.ViewSQLColumn];
                }
                set { this[tableDbTable.ViewSQLColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public int ViewSeq
            {
                get
                {
                    if (IsViewSeqNull())
                        return 0;
                    return (int) this[tableDbTable.ViewSeqColumn];
                }
                set { this[tableDbTable.ViewSeqColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbGroupNull()
            {
                return IsNull(tableDbTable.DbGroupColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDbGroupNull()
            {
                this[tableDbTable.DbGroupColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbDefinitionNull()
            {
                return IsNull(tableDbTable.DbDefinitionColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbDefinitionNull()
            {
                this[tableDbTable.DbDefinitionColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDbPreviousNull()
            {
                return IsNull(tableDbTable.DbPreviousColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDbPreviousNull()
            {
                this[tableDbTable.DbPreviousColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsLabelNull()
            {
                return IsNull(tableDbTable.LabelColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetLabelNull()
            {
                this[tableDbTable.LabelColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDataMethodNull()
            {
                return IsNull(tableDbTable.DataMethodColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDataMethodNull()
            {
                this[tableDbTable.DataMethodColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsScopeStatusNull()
            {
                return IsNull(tableDbTable.ScopeStatusColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetScopeStatusNull()
            {
                this[tableDbTable.ScopeStatusColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsViewSQLNull()
            {
                return IsNull(tableDbTable.ViewSQLColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetViewSQLNull()
            {
                this[tableDbTable.ViewSQLColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsViewSeqNull()
            {
                return IsNull(tableDbTable.ViewSeqColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetViewSeqNull()
            {
                this[tableDbTable.ViewSeqColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbViewRow[] GetDbViewRows()
            {
                if (Table.ChildRelations["DbTableDbView"] == null)
                    return new DbViewRow[0];
                return (DbViewRow[]) GetChildRows(Table.ChildRelations["DbTableDbView"]);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbColumnRow[] GetDbColumnRows()
            {
                if (Table.ChildRelations["DbTableDbColumn"] == null)
                    return new DbColumnRow[0];
                return (DbColumnRow[]) GetChildRows(Table.ChildRelations["DbTableDbColumn"]);
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DbTableRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DbTableRow eventRow;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbTableRowChangeEvent(DbTableRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbTableRow Row
            {
                get { return eventRow; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DbViewDataTable : TypedTableBase<DbViewRow>
        {
            private DataColumn columnDbTable;
            private DataColumn columnViewSQL;
            private DataColumn columnViewSeq;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbViewDataTable()
            {
                TableName = "DbView";
                BeginInit();
                InitClass();
                EndInit();
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbViewDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected DbViewDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DbTableColumn
            {
                get { return columnDbTable; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn ViewSQLColumn
            {
                get { return columnViewSQL; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn ViewSeqColumn
            {
                get { return columnViewSeq; }
            }

            [Browsable(false)]
            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int Count
            {
                get { return Rows.Count; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbViewRow this[int index]
            {
                get { return (DbViewRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbViewRowChangeEventHandler DbViewRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbViewRowChangeEventHandler DbViewRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbViewRowChangeEventHandler DbViewRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DbViewRowChangeEventHandler DbViewRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddDbViewRow(DbViewRow row)
            {
                Rows.Add(row);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbViewRow AddDbViewRow(DbTableRow parentDbTableRowByDbTableDbView, string ViewSQL, int ViewSeq)
            {
                var dbViewRow = (DbViewRow) NewRow();
                var objArray = new object[3]
                {
                    null,
                    ViewSQL,
                    ViewSeq
                };
                if (parentDbTableRowByDbTableDbView != null)
                    objArray[0] = parentDbTableRowByDbTableDbView[0];
                dbViewRow.ItemArray = objArray;
                Rows.Add(dbViewRow);
                return dbViewRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbViewRow FindByDbTable(string DbTable)
            {
                return (DbViewRow) Rows.Find(new object[1]
                {
                    DbTable
                });
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public override DataTable Clone()
            {
                var dbViewDataTable = (DbViewDataTable) base.Clone();
                dbViewDataTable.InitVars();
                return dbViewDataTable;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataTable CreateInstance()
            {
                return new DbViewDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnDbTable = Columns["DbTable"];
                columnViewSQL = Columns["ViewSQL"];
                columnViewSeq = Columns["ViewSeq"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                columnDbTable = new DataColumn("DbTable", typeof (string), null, MappingType.Element);
                Columns.Add(columnDbTable);
                columnViewSQL = new DataColumn("ViewSQL", typeof (string), null, MappingType.Element);
                Columns.Add(columnViewSQL);
                columnViewSeq = new DataColumn("ViewSeq", typeof (int), null, MappingType.Element);
                Columns.Add(columnViewSeq);
                Constraints.Add(new UniqueConstraint("DbTablePKey", new DataColumn[1]
                {
                    columnDbTable
                }, 1 != 0));
                columnDbTable.AllowDBNull = false;
                columnDbTable.Unique = true;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbViewRow NewDbViewRow()
            {
                return (DbViewRow) NewRow();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DbViewRow(builder);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override Type GetRowType()
            {
                return typeof (DbViewRow);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DbViewRowChanged == null)
                    return;
                DbViewRowChanged(this, new DbViewRowChangeEvent((DbViewRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DbViewRowChanging == null)
                    return;
                DbViewRowChanging(this, new DbViewRowChangeEvent((DbViewRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DbViewRowDeleted == null)
                    return;
                DbViewRowDeleted(this, new DbViewRowChangeEvent((DbViewRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DbViewRowDeleting == null)
                    return;
                DbViewRowDeleting(this, new DbViewRowChangeEvent((DbViewRow) e.Row, e.Action));
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void RemoveDbViewRow(DbViewRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DbViewDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (schemaSerializable != null && xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        if (memoryStream1 != null)
                            memoryStream1.Close();
                        if (memoryStream2 != null)
                            memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DbViewRow : DataRow
        {
            private readonly DbViewDataTable tableDbView;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            internal DbViewRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDbView = (DbViewDataTable) Table;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DbTable
            {
                get { return (string) this[tableDbView.DbTableColumn]; }
                set { this[tableDbView.DbTableColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string ViewSQL
            {
                get
                {
                    if (IsViewSQLNull())
                        return string.Empty;
                    return (string) this[tableDbView.ViewSQLColumn];
                }
                set { this[tableDbView.ViewSQLColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int ViewSeq
            {
                get
                {
                    if (IsViewSeqNull())
                        return 0;
                    return (int) this[tableDbView.ViewSeqColumn];
                }
                set { this[tableDbView.ViewSeqColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DbTableRow DbTableRow
            {
                get { return (DbTableRow) GetParentRow(Table.ParentRelations["DbTableDbView"]); }
                set { SetParentRow(value, Table.ParentRelations["DbTableDbView"]); }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsViewSQLNull()
            {
                return IsNull(tableDbView.ViewSQLColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetViewSQLNull()
            {
                this[tableDbView.ViewSQLColumn] = Convert.DBNull;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsViewSeqNull()
            {
                return IsNull(tableDbView.ViewSeqColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetViewSeqNull()
            {
                this[tableDbView.ViewSeqColumn] = Convert.DBNull;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DbViewRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DbViewRow eventRow;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbViewRowChangeEvent(DbViewRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DbViewRow Row
            {
                get { return eventRow; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }

        [XmlSchemaProvider("GetTypedTableSchema")]
        [Serializable]
        public class DevCycleDataTable : TypedTableBase<DevCycleRow>
        {
            private DataColumn columnComponent;
            private DataColumn columnDMVersion;
            private DataColumn columnDMVersionDate;
            private DataColumn columnDevCycle;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DevCycleDataTable()
            {
                TableName = "DevCycle";
                BeginInit();
                InitClass();
                EndInit();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal DevCycleDataTable(DataTable table)
            {
                TableName = table.TableName;
                if (table.CaseSensitive != table.DataSet.CaseSensitive)
                    CaseSensitive = table.CaseSensitive;
                if (table.Locale.ToString() != table.DataSet.Locale.ToString())
                    Locale = table.Locale;
                if (table.Namespace != table.DataSet.Namespace)
                    Namespace = table.Namespace;
                Prefix = table.Prefix;
                MinimumCapacity = table.MinimumCapacity;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            protected DevCycleDataTable(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                InitVars();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn DevCycleColumn
            {
                get { return columnDevCycle; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DMVersionColumn
            {
                get { return columnDMVersion; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataColumn ComponentColumn
            {
                get { return columnComponent; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DataColumn DMVersionDateColumn
            {
                get { return columnDMVersionDate; }
            }

            [DebuggerNonUserCode]
            [Browsable(false)]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int Count
            {
                get { return Rows.Count; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DevCycleRow this[int index]
            {
                get { return (DevCycleRow) Rows[index]; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DevCycleRowChangeEventHandler DevCycleRowChanging;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DevCycleRowChangeEventHandler DevCycleRowChanged;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DevCycleRowChangeEventHandler DevCycleRowDeleting;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public event DevCycleRowChangeEventHandler DevCycleRowDeleted;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void AddDevCycleRow(DevCycleRow row)
            {
                Rows.Add(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DevCycleRow AddDevCycleRow(string DevCycle, int DMVersion, string Component, DateTime DMVersionDate)
            {
                var devCycleRow = (DevCycleRow) NewRow();
                var objArray = new object[4]
                {
                    DevCycle,
                    DMVersion,
                    Component,
                    DMVersionDate
                };
                devCycleRow.ItemArray = objArray;
                Rows.Add(devCycleRow);
                return devCycleRow;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public override DataTable Clone()
            {
                var devCycleDataTable = (DevCycleDataTable) base.Clone();
                devCycleDataTable.InitVars();
                return devCycleDataTable;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataTable CreateInstance()
            {
                return new DevCycleDataTable();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal void InitVars()
            {
                columnDevCycle = Columns["DevCycle"];
                columnDMVersion = Columns["DMVersion"];
                columnComponent = Columns["Component"];
                columnDMVersionDate = Columns["DMVersionDate"];
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            private void InitClass()
            {
                columnDevCycle = new DataColumn("DevCycle", typeof (string), null, MappingType.Element);
                Columns.Add(columnDevCycle);
                columnDMVersion = new DataColumn("DMVersion", typeof (int), null, MappingType.Element);
                Columns.Add(columnDMVersion);
                columnComponent = new DataColumn("Component", typeof (string), null, MappingType.Element);
                Columns.Add(columnComponent);
                columnDMVersionDate = new DataColumn("DMVersionDate", typeof (DateTime), null, MappingType.Element);
                Columns.Add(columnDMVersionDate);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DevCycleRow NewDevCycleRow()
            {
                return (DevCycleRow) NewRow();
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new DevCycleRow(builder);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override Type GetRowType()
            {
                return typeof (DevCycleRow);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanged(DataRowChangeEventArgs e)
            {
                base.OnRowChanged(e);
                if (DevCycleRowChanged == null)
                    return;
                DevCycleRowChanged(this, new DevCycleRowChangeEvent((DevCycleRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowChanging(DataRowChangeEventArgs e)
            {
                base.OnRowChanging(e);
                if (DevCycleRowChanging == null)
                    return;
                DevCycleRowChanging(this, new DevCycleRowChangeEvent((DevCycleRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleted(DataRowChangeEventArgs e)
            {
                base.OnRowDeleted(e);
                if (DevCycleRowDeleted == null)
                    return;
                DevCycleRowDeleted(this, new DevCycleRowChangeEvent((DevCycleRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            protected override void OnRowDeleting(DataRowChangeEventArgs e)
            {
                base.OnRowDeleting(e);
                if (DevCycleRowDeleting == null)
                    return;
                DevCycleRowDeleting(this, new DevCycleRowChangeEvent((DevCycleRow) e.Row, e.Action));
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void RemoveDevCycleRow(DevCycleRow row)
            {
                Rows.Remove(row);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
            {
                var schemaComplexType = new XmlSchemaComplexType();
                var xmlSchemaSequence = new XmlSchemaSequence();
                var databaseSchema = new DatabaseSchema();
                var xmlSchemaAny1 = new XmlSchemaAny();
                xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
                xmlSchemaAny1.MinOccurs = new Decimal(0);
                xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, 0);
                xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny1);
                var xmlSchemaAny2 = new XmlSchemaAny();
                xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                xmlSchemaAny2.MinOccurs = new Decimal(1);
                xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
                xmlSchemaSequence.Items.Add(xmlSchemaAny2);
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "namespace",
                    FixedValue = databaseSchema.Namespace
                });
                schemaComplexType.Attributes.Add(new XmlSchemaAttribute
                {
                    Name = "tableTypeName",
                    FixedValue = "DevCycleDataTable"
                });
                schemaComplexType.Particle = xmlSchemaSequence;
                XmlSchema schemaSerializable = databaseSchema.GetSchemaSerializable();
                if (xs.Contains(schemaSerializable.TargetNamespace))
                {
                    var memoryStream1 = new MemoryStream();
                    var memoryStream2 = new MemoryStream();
                    try
                    {
                        schemaSerializable.Write(memoryStream1);
                        foreach (XmlSchema xmlSchema in xs.Schemas(schemaSerializable.TargetNamespace))
                        {
                            memoryStream2.SetLength(0L);
                            xmlSchema.Write(memoryStream2);
                            if (memoryStream1.Length == memoryStream2.Length)
                            {
                                memoryStream1.Position = 0L;
                                memoryStream2.Position = 0L;
                                do
                                {
                                } while (memoryStream1.Position != memoryStream1.Length &&
                                         memoryStream1.ReadByte() == memoryStream2.ReadByte());
                                if (memoryStream1.Position == memoryStream1.Length)
                                    return schemaComplexType;
                            }
                        }
                    }
                    finally
                    {
                        memoryStream1.Close();
                        memoryStream2.Close();
                    }
                }
                xs.Add(schemaSerializable);
                return schemaComplexType;
            }
        }

        public class DevCycleRow : DataRow
        {
            private readonly DevCycleDataTable tableDevCycle;

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            internal DevCycleRow(DataRowBuilder rb)
                : base(rb)
            {
                tableDevCycle = (DevCycleDataTable) Table;
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string DevCycle
            {
                get
                {
                    if (IsDevCycleNull())
                        return string.Empty;
                    return (string) this[tableDevCycle.DevCycleColumn];
                }
                set { this[tableDevCycle.DevCycleColumn] = value; }
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public int DMVersion
            {
                get
                {
                    if (IsDMVersionNull())
                        return 0;
                    return (int) this[tableDevCycle.DMVersionColumn];
                }
                set { this[tableDevCycle.DMVersionColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public string Component
            {
                get
                {
                    if (IsComponentNull())
                        return string.Empty;
                    return (string) this[tableDevCycle.ComponentColumn];
                }
                set { this[tableDevCycle.ComponentColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DateTime DMVersionDate
            {
                get
                {
                    try
                    {
                        return (DateTime) this[tableDevCycle.DMVersionDateColumn];
                    }
                    catch (InvalidCastException ex)
                    {
                        throw new StrongTypingException(
                            "The value for column 'DMVersionDate' in table 'DevCycle' is DBNull.", ex);
                    }
                }
                set { this[tableDevCycle.DMVersionDateColumn] = value; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public bool IsDevCycleNull()
            {
                return IsNull(tableDevCycle.DevCycleColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetDevCycleNull()
            {
                this[tableDevCycle.DevCycleColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDMVersionNull()
            {
                return IsNull(tableDevCycle.DMVersionColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDMVersionNull()
            {
                this[tableDevCycle.DMVersionColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsComponentNull()
            {
                return IsNull(tableDevCycle.ComponentColumn);
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public void SetComponentNull()
            {
                this[tableDevCycle.ComponentColumn] = Convert.DBNull;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public bool IsDMVersionDateNull()
            {
                return IsNull(tableDevCycle.DMVersionDateColumn);
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public void SetDMVersionDateNull()
            {
                this[tableDevCycle.DMVersionDateColumn] = Convert.DBNull;
            }
        }

        [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        public class DevCycleRowChangeEvent : EventArgs
        {
            private readonly DataRowAction eventAction;
            private readonly DevCycleRow eventRow;

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DevCycleRowChangeEvent(DevCycleRow row, DataRowAction action)
            {
                eventRow = row;
                eventAction = action;
            }

            [DebuggerNonUserCode]
            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            public DevCycleRow Row
            {
                get { return eventRow; }
            }

            [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
            [DebuggerNonUserCode]
            public DataRowAction Action
            {
                get { return eventAction; }
            }
        }
    }
}