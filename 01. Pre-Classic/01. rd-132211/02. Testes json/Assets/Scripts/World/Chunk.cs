using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Chunk : MonoBehaviour {
    public TextMeshProUGUI textMeshPro;

    private string[] blocks = new string[16 * 16 * 16];

    private void Start() {
        this.Add();

        //this.blocks = new int[16, 16];
        this.Load();
    }

    private void Update() {
        
    }

    private void Load() {
        try {
            // Verifica se o arquivo de nível existe.
            if(File.Exists(this.FilePath())) {
                // Lê o conteúdo do arquivo JSON.
                string json = File.ReadAllText(this.FilePath());

                // Converte o JSON para a estrutura LevelData.
                ChunkData chunkData = JsonUtility.FromJson<ChunkData>(json);

                this.blocks = chunkData.blocks;
            }

            this.textMeshPro.text = "Blocks: " + string.Join(", ", this.blocks);
        }
        catch(Exception e) {
            Debug.LogError(e.ToString());
        }
    }

    private void Save() {
        try {
            // Cria uma instância da estrutura LevelData.
            ChunkData chunkData = new ChunkData();
            chunkData.blocks = this.blocks;

            // Converte a estrutura LevelData em JSON.
            string json = JsonUtility.ToJson(chunkData);

            // Escreve o JSON no arquivo.
            File.WriteAllText(this.FilePath(), json);
        }
        catch(Exception e) {
            Debug.LogError(e.ToString());
        }
    }

    private string FilePath() {
        // Nome da pasta onde o arquivo será salvo.
        string folder = ".mineclone";

        // Obtém o caminho da pasta de dados do aplicativo.
        string folderRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // Combina os caminhos da pasta de dados do aplicativo com o nome da pasta.
        string folderPath = Path.Combine(folderRoaming, folder);

        // Verifica se a pasta destino existe, se não, cria-a.
        if(!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }

        // Nome do arquivo onde os dados serão salvos.
        string jsonFile = "level.dat";

        // Combina o caminho da pasta com o nome do arquivo.
        string filePath = Path.Combine(folderPath, jsonFile);

        return filePath;
    }

    private void Add() {
        for(int x = 0; x < 16; x++) {
            for(int y = 0; y < 16; y++) {
                for(int z = 0; z < 16; z++) {
                    int i = (y * 16 + z) * 16 + x;

                    if(y < 41) {
                        this.blocks[i] = "stone";
                    }
                    else if(y == 41) {
                        this.blocks[i] = "grass";
                    }
                    else {
                        this.blocks[i] = "air";
                    }
                }
            }
        }

        this.textMeshPro.text = "Blocks: " + this.blocks.ToString();

        this.Save();
    }
}

[System.Serializable]
public class ChunkData {
    public string[] blocks;
}
