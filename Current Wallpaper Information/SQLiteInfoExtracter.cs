using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace Current_Wallpaper_Information
{
    public class SQLiteInfoExtracter
    {

        private static readonly string DB_PATH = 
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + 
            @"\Current Wallpaper Information\cwi.sqlite";
        private static readonly string DB_CONNNECTION_STRING = "data source=" + DB_PATH;

        static SQLiteInfoExtracter()
        {
            CreateDBPathIfNotExists();
            CreateTablesIfNotExists();
        }

        public struct WallpaperInfo
        {
            public string FileLocation;
            public string SourceLink;
            public string Location;
            public string Description;

            public string Stringify()
            {
                string filenameData = "File Location: " + this.FileLocation;
                string sourcelinkData = (this.SourceLink != null) ? "Source: " + this.SourceLink : null;
                string location = (this.Location != null)? "Location: " + this.Location : null;
                string description = (this.Description != null) ? "Description: " + this.Description : null;
                return filenameData + "\n" + sourcelinkData + "\n" + location + "\n" + description;
            }

            public void Save()
            {
                using var con = new SqliteConnection(DB_CONNNECTION_STRING);
                con.Open();

                using var cmd = new SqliteCommand(@"
                    REPLACE INTO 'Images' VALUES(@filelocation, @sourcelink, @location, @description);
                ", con);

                cmd.Parameters.AddWithValue(@"filelocation", this.FileLocation.ToLower());
                addNullableParam("@sourcelink", this.SourceLink);
                addNullableParam("@location", this.Location);
                addNullableParam("@description", this.Description);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                con.Close();

                void addNullableParam(string parameterKey, string parameterValue)
                {
                    if (parameterValue == null)
                    {
                        cmd.Parameters.AddWithValue(parameterKey, DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(parameterKey, parameterValue);
                    }
                }
            }

        }

        public static WallpaperInfo GetFileInformation(string filePath)
        {

            filePath = filePath.ToLower();

            WallpaperInfo wallpaperInfo = new WallpaperInfo();
            string selectStatement = "SELECT * FROM Images WHERE FileLocation = '" + filePath + "'";

            using var con = new SqliteConnection(DB_CONNNECTION_STRING);
            con.Open();

            using var cmd = new SqliteCommand(selectStatement, con);

            using SqliteDataReader rdr = cmd.ExecuteReader();

            wallpaperInfo.FileLocation = filePath;

            while (rdr.Read())
            {
                wallpaperInfo.SourceLink = ParseReaderOrdinalString(rdr, 1);
                wallpaperInfo.Location = ParseReaderOrdinalString(rdr, 2);
                wallpaperInfo.Description = ParseReaderOrdinalString(rdr, 3);
            }

            con.Close();

            return wallpaperInfo;
        }

        private static void CreateDBPathIfNotExists()
        {
            string pathToDBDirectory = Path.GetDirectoryName(DB_PATH);
            if (!Directory.Exists(pathToDBDirectory))
            {
                Directory.CreateDirectory(pathToDBDirectory);
            }
        }

        private static void CreateTablesIfNotExists()
        {
            using var con = new SqliteConnection(DB_CONNNECTION_STRING);
            con.Open();

            using var cmd = new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS 'Images'(
	                'FileLocation' TEXT NOT NULL,
                    'SourceLink' TEXT,
                    'Location' TEXT,
                    'Description' TEXT,
                    PRIMARY KEY('FileLocation')
                );
            ", con);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        private static string ParseReaderOrdinalString(SqliteDataReader rdr, int ordinal)
        {
            if (rdr.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return rdr.GetString(ordinal);
            }
        }

    }
}
