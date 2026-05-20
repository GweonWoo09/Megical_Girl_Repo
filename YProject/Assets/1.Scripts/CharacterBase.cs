using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public string charName;
    public int charLevel;
    

    public void CharacterSelecter(int level)
    {
        int curLevel = level;

        switch (curLevel)
        {
            case 1:
                charName = "레벨1 마법소녀";
                charLevel = 1;
                break;
            case 2:
                charName = "레벨2 마법소녀";
                charLevel = 2;
                break;
            case 3:
                charName = "레벨3 마법소녀";
                charLevel = 3;
                break;
            case 4:
                charName = "레벨4 마법소녀";
                charLevel = 4;
                break;
            case 5:
                charName = "레벨5 마법소녀";
                charLevel = 5;
                break;
        }
    }
}
