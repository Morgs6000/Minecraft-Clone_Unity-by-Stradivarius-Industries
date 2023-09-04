using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveHandler : MonoBehaviour {
    // Nome do arquivo onde os dados serão salvos.
    private static string level = "level.json";

    /*
    [System.Serializable]
    public struct LevelData {
        public string[,,] blocks;

        public LevelData(string[,,] blocks) {
            this.blocks = blocks;
        }
    }
    */

    public static void Load() {
        // Verifica se o arquivo de nível existe.
        if(File.Exists(level)) {
            // Lê o conteúdo do arquivo JSON.
            string json = File.ReadAllText(level);

            // Converte o JSON para a estrutura LevelData.
            //LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        }
    }

    public static void Save() {
        // Nome da pasta onde o arquivo será salvo.
        string folder = ".mineclone";

        // Obtém o caminho da pasta de dados do aplicativo.
        string folderRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // Combina os caminhos da pasta de dados do aplicativo com o nome da pasta.
        string pathFolder = Path.Combine(folderRoaming, folder);

        // Verifica se a pasta destino existe, se não, cria-a.
        if(!Directory.Exists(pathFolder)) {
            Directory.CreateDirectory(pathFolder);
        }

        // Combina o caminho da pasta com o nome do arquivo.
        string pathFile = Path.Combine(pathFolder, level);

        // Cria uma instância da estrutura LevelData.
        //LevelData levelData = new LevelData(Chunk.blocks);

        // Converte a estrutura LevelData em JSON.
        //string json = JsonUtility.ToJson(levelData);

        // Escreve o JSON no arquivo.
        //File.WriteAllText(pathFile, json);
    }
}
