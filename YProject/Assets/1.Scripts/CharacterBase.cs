using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public string charName;
    public int charLevel;
    public int charPrice;
    public int charProbablity = 100;

    public void CharacterSelecter(int level)
    {
        int curLevel = level;

        switch (curLevel)
        {
            case 1:
                charName = "레벨1 마법소녀";
                charLevel = 1;
                charPrice = 50;
                charProbablity -= 5;
                break;
            case 2:
                charName = "레벨2 마법소녀";
                charLevel = 2;
                charPrice = 100;
                charProbablity -= 5;
                break;
            case 3:
                charName = "레벨3 마법소녀";
                charLevel = 3;
                charPrice = 250;
                charProbablity -= 5;
                break;
            case 4:
                charName = "레벨4 마법소녀";
                charLevel = 3;
                charPrice = 250;
                charProbablity -= 10;
                break;
            case 5:
                charName = "레벨5 마법소녀";
                charLevel = 3;
                charPrice = 250;
                charProbablity -= 10;
                break;
        }
    }
}
