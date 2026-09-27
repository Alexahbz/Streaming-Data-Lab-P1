using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{

    static public void SavePartyButtonPressed()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/partySave.txt");

        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            writer.WriteLine(pc.classID);
            writer.WriteLine(pc.health);
            writer.WriteLine(pc.mana);
            writer.WriteLine(pc.strength);
            writer.WriteLine(pc.agility);
            writer.WriteLine(pc.wisdom);

            writer.WriteLine(pc.equipment.Count);

            foreach (int equipID in pc.equipment)
            {
                writer.WriteLine(equipID);
            }
        }

        writer.Close();
    }

    static public void LoadPartyButtonPressed()
    {
        StreamReader reader = new StreamReader(Application.persistentDataPath + "/partySave.txt");

        GameContent.partyCharacters.Clear();

        while (!reader.EndOfStream)
        {
            int classID = int.Parse(reader.ReadLine());
            int health = int.Parse(reader.ReadLine());
            int mana = int.Parse(reader.ReadLine());
            int strength = int.Parse(reader.ReadLine());
            int agility = int.Parse(reader.ReadLine());
            int wisdom = int.Parse(reader.ReadLine());

            PartyCharacter pc = new PartyCharacter(classID, health, mana, strength, agility, wisdom);

            int equipmentCount = int.Parse(reader.ReadLine());

            for (int i = 0; i < equipmentCount; i++)
            {
                int equipID = int.Parse(reader.ReadLine());
                pc.equipment.AddLast(equipID);
            }

            GameContent.partyCharacters.AddLast(pc);
        }

        reader.Close();

        GameContent.RefreshUI();
    }

}


#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{

    static List<string> listOfPartyNames;
    static List<LinkedList<PartyCharacter>> listOfParties;


    static public void GameStart()
    {
        listOfPartyNames = new List<string>();
        listOfParties = new List<LinkedList<PartyCharacter>>();


        LoadAllPartiesFromFile();
        GameContent.RefreshUI();
    }

    static void SaveAllPartiesToFile()
    {
        StreamWriter writer = new StreamWriter(
            Application.persistentDataPath + "/partySaves.txt"
        );

        writer.WriteLine(listOfParties.Count);

        for (int i = 0; i < listOfParties.Count; i++)
        {
            // Save the party name
            writer.WriteLine(listOfPartyNames[i]);

            // Save how many characters are in this party
            writer.WriteLine(listOfParties[i].Count);

            foreach (PartyCharacter pc in listOfParties[i])
            {
                writer.WriteLine(pc.classID);
                writer.WriteLine(pc.health);
                writer.WriteLine(pc.mana);
                writer.WriteLine(pc.strength);
                writer.WriteLine(pc.agility);
                writer.WriteLine(pc.wisdom);

                // Save equipment count
                writer.WriteLine(pc.equipment.Count);

                // Save each equipment ID
                foreach (int equipID in pc.equipment)
                {
                    writer.WriteLine(equipID);
                }
            }
        }

        writer.Close();
    }

    static void LoadAllPartiesFromFile()
    {
        string filePath = Application.persistentDataPath + "/partySaves.txt";

        if (!File.Exists(filePath))
        {
            return;
        }

        StreamReader reader = new StreamReader(filePath);

        int numberOfParties = int.Parse(reader.ReadLine());

        for (int i = 0; i < numberOfParties; i++)
        {
            // Load party name
            string partyName = reader.ReadLine();
            listOfPartyNames.Add(partyName);

            // Load number of characters
            int characterCount = int.Parse(reader.ReadLine());

            LinkedList<PartyCharacter> loadedParty =
                new LinkedList<PartyCharacter>();

            for (int j = 0; j < characterCount; j++)
            {
                int classID = int.Parse(reader.ReadLine());
                int health = int.Parse(reader.ReadLine());
                int mana = int.Parse(reader.ReadLine());
                int strength = int.Parse(reader.ReadLine());
                int agility = int.Parse(reader.ReadLine());
                int wisdom = int.Parse(reader.ReadLine());

                PartyCharacter pc = new PartyCharacter(
                    classID,
                    health,
                    mana,
                    strength,
                    agility,
                    wisdom
                );

                int equipmentCount = int.Parse(reader.ReadLine());

                for (int k = 0; k < equipmentCount; k++)
                {
                    int equipID = int.Parse(reader.ReadLine());
                    pc.equipment.AddLast(equipID);
                }

                loadedParty.AddLast(pc);
            }

            listOfParties.Add(loadedParty);
        }

        reader.Close();
    }

    static public List<string> GetListOfPartyNames()
    {
        return listOfPartyNames;
    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        int partyIndex = listOfPartyNames.IndexOf(selectedName);

        if (partyIndex >= 0)
        {
            GameContent.partyCharacters.Clear();

            foreach (PartyCharacter pc in listOfParties[partyIndex])
            {
                PartyCharacter newPC = new PartyCharacter(
                    pc.classID,
                    pc.health,
                    pc.mana,
                    pc.strength,
                    pc.agility,
                    pc.wisdom
                );

                foreach (int equipID in pc.equipment)
                {
                    newPC.equipment.AddLast(equipID);
                }

                GameContent.partyCharacters.AddLast(newPC);
            }
        }

        GameContent.RefreshUI();
    }

    static public void SavePartyButtonPressed()
    {
        string partyName = GameContent.GetPartyNameFromInput();

        listOfPartyNames.Add(partyName);

        LinkedList<PartyCharacter> savedParty = new LinkedList<PartyCharacter>();

        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            PartyCharacter newPC = new PartyCharacter(
                pc.classID,
                pc.health,
                pc.mana,
                pc.strength,
                pc.agility,
                pc.wisdom
            );

            foreach (int equipID in pc.equipment)
            {
                newPC.equipment.AddLast(equipID);
            }

            savedParty.AddLast(newPC);
        }

        listOfParties.Add(savedParty);

        SaveAllPartiesToFile();

        GameContent.RefreshUI();
    }

    static public void DeletePartyButtonPressed()
    {
        string selectedName = GameContent.GetSelectedPartyName();

        int partyIndex = listOfPartyNames.IndexOf(selectedName);

        if (partyIndex >= 0)
        {
            listOfPartyNames.RemoveAt(partyIndex);
            listOfParties.RemoveAt(partyIndex);

            SaveAllPartiesToFile();
        }

        GameContent.RefreshUI();
    }
}
#endregion


