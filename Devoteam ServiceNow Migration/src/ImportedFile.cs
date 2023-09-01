namespace Devoteam_ServiceNow_Migration
{
    public class ImportedFile
    {
        public string folder { get; set; }
        public string name { get; set; }
        public string param { get; set; }

        public ImportedFile(string folder, string name, string param)
        {
            this.folder = folder;
            this.name = name;
            this.param = param;
        }
    }
}