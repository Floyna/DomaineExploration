using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    public SpellData spellData;

    private RectTransform rectTransform;
    private Mouse mouse;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision);
    }

    void Start()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        mouse = FindObjectOfType<Mouse>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject spellDrag = new GameObject();
        Destroy(spellDrag.GetComponent<Transform>());
        spellDrag.AddComponent<Image>();
        spellDrag.GetComponent<Image>().sprite = spellData.visuelSpell;
        spellDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 75);
        GameObject go = Instantiate(spellDrag, canvas.transform);
        Destroy(spellDrag);
        go.name = spellData.spellName;
        go.tag = "SpellDrag";
        go.transform.position = mouse.mousePos;
        rectTransform = go.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (mouse.canDropSpell)
        {
            mouse.currentSlot.GetComponent<SlotSpell>().SetSpellInSlot(spellData);
        }

        Destroy(GameObject.FindGameObjectWithTag("SpellDrag"));
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Appuie sur :" + gameObject);  
    }
}
