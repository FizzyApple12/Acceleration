﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance { get; private set; }

    public Save save;
    public string saveName;
    
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadSave();
        } else {
            Destroy(gameObject);
        }
    }

    public void loadSave() {
        save.unlocked = 1;
        if (File.Exists(Application.persistentDataPath + saveName)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveName, FileMode.Open);
            save = (Save) binaryFormatter.Deserialize(file);
            file.Close();
            save.firstPlay = false;
        } else {
            save.unlocked = 1;
            save.firstPlay = true;
            save.name = "user";
        }

        saveSave();
    }

    public void saveSave() {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveName);
        binaryFormatter.Serialize(file, save);
        file.Close();
    }
}
