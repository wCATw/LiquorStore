using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace LiquorStore;
  public class CustomLiquorCase : MonoBehaviour
  {
    public string brand;
    public InteractionRaycast raycast;
    public GameObject drinkPrefab;
    public Collider trigger;
    public GameObject[] bottles;
    private PlayMakerFSM playerFunctions;
    private FsmBool helmet;
    private FsmBool handLeft;
    private FsmBool guiUse;
    private bool mouseOver;
    private bool canDrink = true;

    private void Start()
    {
      this.playerFunctions = ((Component) GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera")).GetComponent<PlayMakerFSM>();
      this.helmet = FsmVariables.GlobalVariables.FindFsmBool("PlayerHelmet");
      this.handLeft = FsmVariables.GlobalVariables.FindFsmBool("PlayerHandLeft");
      this.guiUse = FsmVariables.GlobalVariables.FindFsmBool("GUIuse");
    }

    private void OnCollisionEnter()
    {
      if (((IEnumerable<GameObject>) this.bottles).Where<GameObject>((Func<GameObject, bool>) (x => x.activeInHierarchy)).ToArray<GameObject>().Length == 0)
        return;
      MasterAudio.PlaySound3DAndForget("Bottles", ((Component) this).transform, true, 1f, new float?(1f), 0.0f, "bottles_rattle" + Random.Range(1, 3).ToString());
    }

    private void Update()
    {
      if (this.raycast.GetHit(this.trigger) && this.canDrink && ((Behaviour) this.playerFunctions).enabled)
      {
        this.guiUse.Value = true;
        this.mouseOver = true;
        if (this.helmet.Value || this.handLeft.Value || !cInput.GetKeyDown("Use"))
          return;
        this.StartCoroutine(this.Drink());
      }
      else
      {
        if (!this.mouseOver)
          return;
        this.guiUse.Value = false;
        this.mouseOver = false;
      }
    }

    private IEnumerator Drink()
    {
      CustomLiquorCase customLiquorCase = this;
      customLiquorCase.canDrink = false;
      GameObject[] array = ((IEnumerable<GameObject>) customLiquorCase.bottles).Where<GameObject>((Func<GameObject, bool>) (x => x.activeInHierarchy)).ToArray<GameObject>();
      if (array.Length != 0)
      {
        ((Behaviour) customLiquorCase.playerFunctions).enabled = false;
        GameObject gameObject = Object.Instantiate<GameObject>(customLiquorCase.drinkPrefab);
        gameObject.GetComponent<DrinkBehaviour>().trigger = (Collider) null;
        gameObject.SetActive(true);
        array[Random.Range(0, array.Length - 1)].SetActive(false);
      }
      if (((IEnumerable<GameObject>) customLiquorCase.bottles).Where<GameObject>((Func<GameObject, bool>) (x => x.activeInHierarchy)).ToArray<GameObject>().Length == 0)
      {
        ((Component) customLiquorCase).GetComponent<Rigidbody>().mass = 1f;
        ((Object) customLiquorCase).name = "empty(itemx)";
      }
      yield return (object) new WaitForSeconds(7f);
      ((Behaviour) customLiquorCase.playerFunctions).enabled = true;
      customLiquorCase.canDrink = ((IEnumerable<GameObject>) customLiquorCase.bottles).Where<GameObject>((Func<GameObject, bool>) (x => x.activeInHierarchy)).ToArray<GameObject>().Length != 0;
      yield return (object) null;
    }
  }
