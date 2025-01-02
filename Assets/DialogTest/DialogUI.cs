using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using TMPro;

namespace MySampleEx
{
    /// <summary>
    /// ��ȭâ ���� Ŭ���� 
    /// ��ȭ ������ ���� �б�
    /// ��ȭ ������ UI ����
    /// </summary>
    public class DialogUI : MonoBehaviour
    {
        #region Varialbes
        //XML
        public string xmlFile = "Dialog/Dialog";    //Path
        private XmlNodeList allNodes;

        private Queue<Dialog> dialogs;

        //UI
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI sentenceText;
        public GameObject npcImage;
        public GameObject nextButton;
        #endregion

        private void Start()
        {
            //xml ������ ���� �б�
            LoadDialogXml(xmlFile);

            dialogs = new Queue<Dialog>();
            InitDialog();

            StartDialog(0);
        }

        //Xml ������ �о���̱�
        private void LoadDialogXml(string path)
        {
            TextAsset xmlFile = Resources.Load<TextAsset>(path);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile.text);
            allNodes = xmlDoc.SelectNodes("root/Dialog");
        }

        //�ʱ�ȭ
        private void InitDialog()
        {
            dialogs.Clear();

            npcImage.SetActive(false);
            nameText.text = "";
            sentenceText.text = "";
            nextButton.SetActive(true);
        }

        //��ȭ �����ϱ�
        public void StartDialog(int dialogIndex)
        {
            foreach(XmlNode node in allNodes)
            {
                int num = int.Parse(node["number"].InnerText);
                if(num == dialogIndex)
                {
                    Dialog dialog = new Dialog();
                    dialog.number = num;
                    dialog.character = int.Parse (node["character"].InnerText);
                    dialog.name = node["name"].InnerText;
                    dialog.sentence = node["sentence"].InnerText;

                    dialogs.Enqueue(dialog);
                }
            }

            //ù��° ��ȭ�� �����ش�
            DrawNextDialog();
        }

        //���� ��ȭ�� �����ش� - Queue dialogs���� �ϳ� ������ �����ش�
        public void DrawNextDialog()
        {
            //dialogs üũ
            if(dialogs.Count == 0)
            {
                EndDialog();
                return;
            }


            //dialogs���� �ϳ� �����´�
            Dialog dialog = dialogs.Dequeue();

            if(dialog.character > 0)
            {
                npcImage.SetActive(true);
                npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(
                    "Dialog/Npc/npc0" + dialog.character.ToString());
            }
            else  //dialog.character <= 0
            {
                npcImage.SetActive(false);
            }

            nextButton.SetActive(false);

            nameText.text = dialog.name;
            StartCoroutine(typingSentence(dialog.sentence));
        }

        //�ؽ�Ʈ Ÿ���� ����
        IEnumerator typingSentence(string typingText)
        {
            sentenceText.text = "";

            foreach(char latter in typingText)
            {
                sentenceText.text += latter;
                yield return new WaitForSeconds(0.03f);
            }

            nextButton.SetActive(true);
        }


        //��ȭ ����
        private void EndDialog()
        {
            InitDialog();

            //��ȭ ����� �̺�Ʈ ó��
            //...
        }
    }  
}