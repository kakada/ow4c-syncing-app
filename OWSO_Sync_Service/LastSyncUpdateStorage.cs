using System;
using System.IO;

namespace OWSO_Sync_Service
{
    class LastSyncUpdateStorage
    {
        private const String FILENAME = "date.cfg";
        private readonly String _filePath;

        public LastSyncUpdateStorage()
        {
            _filePath = FILENAME;
        }

        public void storeLastUpdateSync(int timestamp)
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
                bw.Write(timestamp);
            }
            catch (IOException e)
            {
                Logger.getInstance().logError(this, e);
                return;
            }
            bw.Close();
        }

        public int getLastUpdateSync()
        {
            int timestamp = 0;
            BinaryReader br = null;
            try
            {
                FileStream f = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Logger.getInstance().log(this, "Read File Path: " + f.Name);
                br = new BinaryReader(f);
                timestamp = br.ReadInt32();
                Logger.getInstance().log(this, "Read Timestamp : " + timestamp);
            }
            catch (IOException e)
            {
                Logger.getInstance().logError(this, e);
            }

            if (br != null)
            {
                br.Close();
            }

            return timestamp;
        }
    }
}
