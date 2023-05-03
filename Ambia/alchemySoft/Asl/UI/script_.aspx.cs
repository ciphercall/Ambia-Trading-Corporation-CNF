 
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace alchemySoft.Asl.UI
{
    public partial class script_ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ScriptDatabase();
        }
        public string ScriptDatabase()
        {
            var sb = new StringBuilder();

            Scripter scripter = new Scripter(myServer);
            Database myAdventureWorks = myServer.Databases["AdventureWorks"];
            /* With ScriptingOptions you can specify different scripting
             * options, for example to include IF NOT EXISTS, DROP
             * statements, output location etc*/
            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptDrops = true;
            scriptOptions.IncludeIfNotExists = true;

            foreach (Table myTable in myAdventureWorks.Tables)
            {
                /* Generating IF EXISTS and DROP command for tables */
                StringCollection tableScripts = myTable.Script(scriptOptions);
                foreach (string script in tableScripts)
                    Console.WriteLine(script);

                /* Generating CREATE TABLE command */
                tableScripts = myTable.Script();
                foreach (string script in tableScripts)
                    Console.WriteLine(script);

                IndexCollection indexCol = myTable.Indexes;
                foreach (Index myIndex in myTable.Indexes)
                {
                    /* Generating IF EXISTS and DROP command for table indexes */
                    StringCollection indexScripts = myIndex.Script(scriptOptions);
                    foreach (string script in indexScripts)
                        Console.WriteLine(script);

                    /* Generating CREATE INDEX command for table indexes */
                    indexScripts = myIndex.Script();
                    foreach (string script in indexScripts)
                        Console.WriteLine(script);
                }
            }
            return sb.ToString();
        }
    }
}