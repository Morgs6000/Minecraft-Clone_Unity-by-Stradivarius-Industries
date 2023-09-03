using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveHandler : MonoBehaviour {
    public static void Load(string chunkName) {
        try {
            // Verifica se o arquivo de nível existe.
            if(File.Exists(FilePath(chunkName))) {
                // Lê o conteúdo do arquivo JSON.
                string json = File.ReadAllText(FilePath(chunkName));

                // Converte o JSON para a estrutura LevelData.
                ChunkData chunkData = JsonUtility.FromJson<ChunkData>(json);
                Chunk.blocks = chunkData.blocks;
            }
        }
        catch(Exception e) {
            Debug.LogError(e.ToString());
        }
    }

    public static void Save(string chunkName) {
        try {
            // Cria uma instância da estrutura LevelData.
            ChunkData chunkData = new ChunkData();
            chunkData.blocks = Chunk.blocks;

            // Converte a estrutura LevelData em JSON.
            string json = JsonUtility.ToJson(chunkData);

            // Escreve o JSON no arquivo.
            File.WriteAllText(FilePath(chunkName), json);
        }
        catch(Exception e) {
            Debug.LogError(e.ToString());
        }
    }

    private static string FilePath(string chunkName) {
        // Nome da pasta onde o arquivo será salvo.
        string folder = ".mineclone/Chunks";

        // Obtém o caminho da pasta de dados do aplicativo.
        string folderRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // Combina os caminhos da pasta de dados do aplicativo com o nome da pasta.
        string folderPath = Path.Combine(folderRoaming, folder);

        // Verifica se a pasta destino existe, se não, cria-a.
        if(!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }

        // Nome do arquivo onde os dados serão salvos.
        string jsonFile = chunkName + ".dat";

        // Combina o caminho da pasta com o nome do arquivo.
        string filePath = Path.Combine(folderPath, jsonFile);

        return filePath;
    }
}

[System.Serializable]
public class ChunkData {
    public string[] blocks;
}
