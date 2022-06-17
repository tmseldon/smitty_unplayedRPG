using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] GameObject _aIResponseGroup;
        [SerializeField] TextMeshProUGUI _aiText;
        [SerializeField] TextMeshProUGUI _speakerNameText;
        [SerializeField] Image _avatarSpeaker;
        [SerializeField] Button _nextButton;
        [SerializeField] Button _quitButton;

        [SerializeField] Transform _buttonsGroup;
        [SerializeField] GameObject _choiceButtonPrefab;

        private PlayerConversant _playerConversant;


        private void Awake()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        private void OnEnable()
        {
            _playerConversant.OnConversationUpdated += UpdateUI;
        }

        private void OnDestroy()
        {
            _playerConversant.OnConversationUpdated -= UpdateUI;
        }

        private void Start()
        {
            _nextButton.onClick.AddListener(_playerConversant.Next);
            _quitButton.onClick.AddListener(_playerConversant.Quit);

            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(_playerConversant.IsActive());

            if (!_playerConversant.IsActive())
            {
                return;
            }

            _aIResponseGroup.SetActive(!_playerConversant.IsChoosing);
            _buttonsGroup.gameObject.SetActive(_playerConversant.IsChoosing);

            if (_playerConversant.IsChoosing)
            {
                CreateButtonList();
                UpdateAvatarInfoUI();
            }
            else
            {
                _aiText.SetText(_playerConversant.GetText());
                _nextButton.gameObject.SetActive(_playerConversant.HasNext());
                UpdateAvatarInfoUI();
            }
        }

        private void UpdateAvatarInfoUI()
        {
            Sprite updateAvatar;
            string speakerName = _playerConversant.GetInfoAvatar(out updateAvatar);
            _speakerNameText.SetText(speakerName);
            _avatarSpeaker.sprite = updateAvatar;
        }

        private void CreateButtonList()
        {
            //REset buttons
            foreach (Transform botoneleccion in _buttonsGroup)
            {
                Destroy(botoneleccion.gameObject);
            }

            //Populate buttons
            foreach (DialogueNode choice in _playerConversant.GetChoices())
            {
                GameObject instantiatedButton = Instantiate(_choiceButtonPrefab, _buttonsGroup);
                TextMeshProUGUI textOnButtom = instantiatedButton.GetComponentInChildren<TextMeshProUGUI>();
                textOnButtom.SetText(choice.TextDialogue);
                Button buttonChoice = instantiatedButton.GetComponentInChildren<Button>();
                buttonChoice.onClick.AddListener(() =>
                       {
                           _playerConversant.SelectChoice(choice);
                       }
                );
            }
        }
    }
}
