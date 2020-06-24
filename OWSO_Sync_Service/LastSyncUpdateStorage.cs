using System;
using System.Globalization;
using System.IO;

namespace OWSO_Sync_Service
{
    class LastSyncUpdateStorage
    {
        private const String FILENAME = "date.cfg";
        private const String DATETIME_FORMAT = "yyyyMMddHHmmssFFF";
        private readonly String _filePath;

        public LastSyncUpdateStorage()
        {
            _filePath = FILENAME;
        }

        public void storeLastUpdateSync(DateTime date)
        {
            BinaryWriter bw;

            try
            {
                FileStream f = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                Logger.getInstance().log(this, "Write File Path: " + f.Name);
                bw = new BinaryWriter(f);
            }
            catch (IOException e)
            {
                Logger.getInstance().logError(this, e);
                return;
            }

            //writing into the file
            try
            {
                bw.Write(date.ToString(DATETIME_FORMAT));
            }
            catch (IOException e)
            {
                Logger.getInstance().logError(this, e);
                return;
            }
            bw.Close();
        }

        public DateTime getLastUpdateSync()
        {
            DateTime time = new DateTime(0);
            BinaryReader br = null;
            try
            {
                FileStream f = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Logger.getInstance().log(this, "Read File Path: " + f.Name);
                br = new BinaryReader(f);
                String dateTimeString = br.ReadString();
                Logger.getInstance().log(this, "DateTimeString : " + dateTimeString);
                if(!(dateTimeString == null || dateTimeString.Equals("")))
                {
                    time = DateTime.ParseExact(dateTimeString, DATETIME_FORMAT, CultureInfo.InvariantCulture);
                }
            }
            catch (IOException e)
            {
                Logger.getInstance().logError(this, e);
            }

            if (br != null)
            {
                br.Close();
            }

            return time;
        }
    }
}
