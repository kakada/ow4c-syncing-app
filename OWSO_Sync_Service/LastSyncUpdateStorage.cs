using System;
using System.IO;

namespace OWSO_Sync_Service
{
    class LastSyncUpdateStorage
    {
        private const String FILENAME = "sync_date.data";
        private readonly String _filePath;

        public LastSyncUpdateStorage()
        {
            _filePath = String.Format(@"{0}\{1}", Environment.CurrentDirectory, FILENAME);
        }

        public void storeLastUpdateSync(int time)
        {
            BinaryWriter bw;

            try
            {
                FileStream f = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                Logger.getInstance().log(this, "Write File Path: " + _filePath);
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
                bw.Write((Int32)time);
            }
            catch (IOException e)
            {
                Logger.getInstance().logError(this, e);
                return;
            }
            bw.Close();
        }

        public long getLastUpdateSync()
        {
            long time = 0;
            BinaryReader br = null;
            try
            {
                FileStream f = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Logger.getInstance().log(this, "Read File Path: " + _filePath);
                br = new BinaryReader(f);
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                time = br.ReadInt32();
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
