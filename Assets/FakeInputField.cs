using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FakeInputField : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TMP_InputField inputField; // 绑定 UI 的 InputField
    public string predefinedText = "Hello, World!"; // 预设文本
    private int currentIndex = 0; // 记录当前显示到第几个字符
    private bool isFocused = false; // 是否处于焦点状态
    public ChatWindowController chatWindowController;

    private FMOD.Studio.EventInstance keyboardSound;

    void Start()
    {
        inputField.text = ""; // 清空输入框
        inputField.readOnly = false; // 设置为只读，避免真实输入
        inputField.onValueChanged.AddListener(BlockRealInput);

        keyboardSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/sfx_ui_type_message_loop");
    }

    void Update()
    {
        if (isFocused && Input.anyKeyDown && !string.IsNullOrEmpty(Input.inputString)) // 仅在 `InputField` 选中时响应按键
        {
            SimulateTyping();
            keyboardSound.start();
        }
        else
        {
            keyboardSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    /// <summary>
    /// 检测键盘是否按下（替代 `Input.anyKeyDown`）
    /// </summary>
    private bool AnyKeyPressed()
    {
        return Input.inputString.Length > 0 ; // 检测任何键盘输入（不受 `EventSystem` 影响）
    }

    /// <summary>
    /// 模拟输入文字
    /// </summary>
    private void SimulateTyping()
    {
        if (predefinedText != null && predefinedText.Length > 0)
        {
            
            if (currentIndex < predefinedText.Length)
            {
                inputField.text += predefinedText[currentIndex]; // 逐个添加预设文字
                var addCount = math.max(1, (int)math.ceil( predefinedText.Length / 8f));
                currentIndex+= addCount;
                if (currentIndex >= predefinedText.Length)
                {
                    currentIndex = predefinedText.Length;
                }
            }
            else
            {
                chatWindowController.updateInputArrow(true);
            }
        }
        else
        {
            inputField.text = "";
        }
    }

    /// <summary>
    /// 当 `InputField` 被选中（点击、Tab 切换）时，允许输入
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        isFocused = true;
    }
    private void BlockRealInput(string input)
    {
        // 始终强制回到当前已输入的预设文本
        inputField.text = predefinedText.Substring(0, currentIndex);
        inputField.caretPosition = currentIndex; // 确保光标始终在末尾
    }
    /// <summary>
    /// 当 `InputField` 失去焦点（点击外部）时，停止输入
    /// </summary>
    public void OnDeselect(BaseEventData eventData)
    {
        isFocused = false;
    }

    public bool isFinishedType()
    {
        return currentIndex >= predefinedText.Length;
    }

    public void Clear()
    {
        currentIndex = 0;
        inputField.textComponent.text = "";
        inputField.text = "";
        predefinedText = "";
    }
}