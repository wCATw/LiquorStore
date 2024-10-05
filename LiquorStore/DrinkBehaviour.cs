// Decompiled with JetBrains decompiler
// Type: LiquorStore.DrinkBehaviour
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using HealthMod;
using HutongGames.PlayMaker;
using MSCLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace LiquorStore
{
  public class DrinkBehaviour : MonoBehaviour
  {
    public string brand = "nivalan";
    public Collider trigger;
    public InteractionRaycast raycast;
    [Header("Drink Variables (preset:beer)")]
    public float weightAdd = 0.0184f;
    public float drunkAdd = 0.02f;
    public float fatigueAdd;
    public float fatigueRateAdd;
    public float urineAdd = 1.3f;
    public float thirstAdd = -5.3f;
    public float hungerAdd = -0.7f;
    public float stressAdd = -3f;
    [Header("Health Mod compatibility")]
    public int drinkMulti = 10;
    [Header("Drink Settings")]
    public bool canCauseBlindness;
    public bool hasCap = true;
    public bool drinkFast;
    public string drinkEmptyName = "empty bottle(Clone)";
    public Vector3 forceAxis;
    public MeshFilter drinkMeshFilter;
    public Mesh openDrinkMesh;
    [Header("{OPTIONAL}")]
    public AudioClip capSFX;
    public AudioClip drinkSFX;
    public AudioClip[] collisionSFX;
    [Header("True and empty collision SFX Array = bottle sounds")]
    public bool useCollisionSounds = true;
    private Rigidbody rigidbody;
    private Animation handAnim;
    private GameObject bottleHand;
    private Transform throwBottle;
    private Camera fpsCam;
    private FsmFloat weight;
    private FsmFloat drunk;
    private FsmFloat fatigue;
    private FsmFloat fatigueRate;
    private FsmFloat urine;
    private FsmFloat thirst;
    private FsmFloat hunger;
    private FsmFloat stress;
    private FsmBool playerDrink;
    private FsmBool playerHandLeft;
    private FsmBool helmet;
    private FsmBool guiUse;
    private FsmGameObject pickedObject;
    private FsmGameObject raycastedObject;
    private PlayMakerFSM playerFunctions;
    private PlayMakerFSM blindness;
    private PlayMakerFSM hand;
    private RaycastHit hitInfo;
    private bool mouseOver;
    public bool isEmpty;
    private MeshRenderer renderer;
    private static bool healthModInstalled;

    private void OnCollisionEnter()
    {
      if (!this.useCollisionSounds || !this.isEmpty)
        return;
      if (this.collisionSFX.Length < 1)
        MasterAudio.PlaySound3DAndForget("BottlesEmpty", ((Component) this).transform, true, 1f, new float?(1f), 0.0f, string.Format("bottle_empty{0}", (object) Random.Range(1, 5)));
      else
        AudioSource.PlayClipAtPoint(this.collisionSFX[Random.Range(0, this.collisionSFX.Length - 1)], ((Component) this).transform.position, 1f);
    }

    private void Start()
    {
      this.weight = FsmVariables.GlobalVariables.FindFsmFloat("PlayerWeight");
      this.drunk = FsmVariables.GlobalVariables.FindFsmFloat("PlayerDrunkAdjusted");
      this.fatigue = FsmVariables.GlobalVariables.FindFsmFloat("PlayerFatigue");
      this.fatigueRate = FsmVariables.GlobalVariables.FindFsmFloat("PlayerFatigueRate");
      this.urine = FsmVariables.GlobalVariables.FindFsmFloat("PlayerUrine");
      this.thirst = FsmVariables.GlobalVariables.FindFsmFloat("PlayerThirst");
      this.hunger = FsmVariables.GlobalVariables.FindFsmFloat("PlayerHunger");
      this.stress = FsmVariables.GlobalVariables.FindFsmFloat("PlayerStress");
      this.playerDrink = FsmVariables.GlobalVariables.FindFsmBool("PlayerDrinkOn");
      this.playerHandLeft = FsmVariables.GlobalVariables.FindFsmBool("PlayerHandLeft");
      this.helmet = FsmVariables.GlobalVariables.FindFsmBool("PlayerHelmet");
      this.guiUse = FsmVariables.GlobalVariables.FindFsmBool("GUIuse");
      this.fpsCam = FsmVariables.GlobalVariables.FindFsmGameObject("POV").Value.GetComponent<Camera>();
      this.handAnim = ((Component) GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/FPSCamera/Drink/Hand")).GetComponent<Animation>();
      this.bottleHand = ((Component) ((Component) this.handAnim).transform.Find("HandBottles")).gameObject;
      this.throwBottle = GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/FPSCamera/ThrowBottle");
      this.hitInfo = new RaycastHit();
      this.playerFunctions = ((Component) GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera")).GetComponent<PlayMakerFSM>();
      this.blindness = ((IEnumerable<PlayMakerFSM>) ((Component) this.fpsCam).GetComponentsInChildren<PlayMakerFSM>(true)).First<PlayMakerFSM>((Func<PlayMakerFSM, bool>) (x => x.FsmName == "Blindness"));
      this.hand = ((Component) GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/1Hand_Assemble/Hand")).GetComponent<PlayMakerFSM>();
      this.pickedObject = this.hand.FsmVariables.FindFsmGameObject("PickedObject");
      this.raycastedObject = this.hand.FsmVariables.FindFsmGameObject("RaycastHitObject");
      this.rigidbody = ((Component) ((Component) this).transform).GetComponent<Rigidbody>();
      this.renderer = ((Component) ((Component) this).transform).GetComponent<MeshRenderer>();
      if (Object.op_Equality((Object) this.trigger, (Object) null))
        this.StartCoroutine(this.Drink());
      if (this.isEmpty)
        ((Object) ((Component) this).gameObject).name = this.drinkEmptyName;
      if (!ModLoader.IsModPresent("Health"))
        return;
      DrinkBehaviour.healthModInstalled = true;
    }

    private void Update()
    {
      if (this.raycast.GetHit(this.trigger))
      {
        if (((Behaviour) this.playerFunctions).enabled && !this.playerHandLeft.Value && !this.helmet.Value && !this.isEmpty)
        {
          this.guiUse.Value = true;
          this.mouseOver = true;
          if (cInput.GetKeyDown("Use"))
            this.StartCoroutine(this.Drink());
        }
        else if (this.mouseOver)
        {
          this.guiUse.Value = false;
          this.mouseOver = false;
        }
      }
      if (this.rigidbody.isKinematic && !this.rigidbody.useGravity && !this.rigidbody.detectCollisions)
      {
        this.rigidbody.isKinematic = false;
        this.rigidbody.useGravity = true;
        this.rigidbody.detectCollisions = true;
      }
      if (((Renderer) this.renderer).enabled)
        return;
      ((Renderer) this.renderer).enabled = true;
    }

    private void HealthModThing()
    {
      if ((double) Health.drunk.Value <= 4.0)
        return;
      Health.poisonCounter += this.drinkMulti;
    }

    public IEnumerator Drink()
    {
      DrinkBehaviour drinkBehaviour = this;
      if (Object.op_Equality((Object) drinkBehaviour.pickedObject.Value, (Object) ((Component) drinkBehaviour).gameObject))
      {
        drinkBehaviour.raycastedObject.Value = (GameObject) null;
        drinkBehaviour.pickedObject.Value = (GameObject) null;
        ((Joint) ((Component) drinkBehaviour.hand).GetComponent<FixedJoint>()).connectedBody = (Rigidbody) null;
      }
      ((Behaviour) drinkBehaviour.playerFunctions).enabled = false;
      drinkBehaviour.isEmpty = true;
      if (Object.op_Equality((Object) drinkBehaviour.drinkSFX, (Object) null))
        MasterAudio.PlaySound3DAndForget("PlayerMisc", ((Component) drinkBehaviour.fpsCam).transform, true, 1f, new float?(1f), 0.0f, "drinking");
      else
        AudioSource.PlayClipAtPoint(drinkBehaviour.drinkSFX, ((Component) drinkBehaviour.fpsCam).transform.position, 1f);
      drinkBehaviour.playerDrink.Value = true;
      drinkBehaviour.playerHandLeft.Value = true;
      ((Component) drinkBehaviour.handAnim).gameObject.SetActive(true);
      drinkBehaviour.bottleHand.gameObject.SetActive(true);
      drinkBehaviour.rigidbody.isKinematic = true;
      drinkBehaviour.rigidbody.velocity = Vector3.zero;
      ((Component) drinkBehaviour).transform.SetParent(((Component) drinkBehaviour.handAnim).transform, false);
      ((Component) drinkBehaviour).transform.localPosition = new Vector3(0.3400203f, 0.1535446f, -0.04791023f);
      ((Component) drinkBehaviour).transform.localEulerAngles = new Vector3(21.22298f, 179.3698f, 178.974f);
      ((Component) ((Component) drinkBehaviour).transform).gameObject.layer = LayerMask.NameToLayer("Player");
      drinkBehaviour.drinkMeshFilter.mesh = drinkBehaviour.openDrinkMesh;
      if (drinkBehaviour.hasCap)
      {
        if (Object.op_Equality((Object) drinkBehaviour.capSFX, (Object) null))
          MasterAudio.PlaySound3DAndForget("HouseFoley", ((Component) drinkBehaviour.handAnim).transform.parent, false, 1f, new float?(1f), 0.0f, "bottle_cap");
        else
          AudioSource.PlayClipAtPoint(drinkBehaviour.capSFX, ((Component) drinkBehaviour.fpsCam).transform.position, 1f);
      }
      drinkBehaviour.weight.Value += drinkBehaviour.weightAdd;
      drinkBehaviour.fatigueRate.Value += drinkBehaviour.fatigueRateAdd;
      drinkBehaviour.handAnim.Play(drinkBehaviour.drinkFast ? "drink_rotate_short" : "drink_rotate", (PlayMode) 4);
      while (drinkBehaviour.handAnim.isPlaying)
      {
        drinkBehaviour.drunk.Value += drinkBehaviour.drunkAdd * Time.deltaTime;
        drinkBehaviour.urine.Value += drinkBehaviour.urineAdd * Time.deltaTime;
        drinkBehaviour.thirst.Value += drinkBehaviour.thirstAdd * Time.deltaTime;
        drinkBehaviour.hunger.Value += drinkBehaviour.hungerAdd * Time.deltaTime;
        drinkBehaviour.fatigue.Value += drinkBehaviour.fatigueAdd * Time.deltaTime;
        if (DrinkBehaviour.healthModInstalled)
          drinkBehaviour.HealthModThing();
        yield return (object) new WaitForEndOfFrame();
      }
      MasterAudio.StopAllOfSound("PlayerMisc");
      yield return (object) new WaitForSeconds(0.1f);
      ((Component) drinkBehaviour).transform.SetParent((Transform) null);
      ((Component) drinkBehaviour).transform.position = drinkBehaviour.throwBottle.position;
      ((Component) drinkBehaviour).transform.eulerAngles = drinkBehaviour.throwBottle.eulerAngles;
      ((Object) ((Component) ((Component) drinkBehaviour).transform).gameObject).name = drinkBehaviour.drinkEmptyName;
      drinkBehaviour.rigidbody.isKinematic = false;
      drinkBehaviour.rigidbody.velocity = Vector3.zero;
      drinkBehaviour.rigidbody.AddRelativeForce(Vector3.op_Multiply(drinkBehaviour.forceAxis, 150f), (ForceMode) 0);
      drinkBehaviour.stress.Value += drinkBehaviour.stressAdd;
      MasterAudio.PlaySound3DAndForget("Burb", ((Component) drinkBehaviour.handAnim).transform.parent, false, 1f, new float?(1f), 0.0f, string.Format("burb0{0}", (object) Random.Range(1, 3)));
      ((Component) ((Component) drinkBehaviour).transform).tag = "ITEM";
      ((Component) ((Component) drinkBehaviour).transform).gameObject.layer = LayerMask.NameToLayer("Parts");
      drinkBehaviour.playerDrink.Value = false;
      drinkBehaviour.playerHandLeft.Value = false;
      ((Component) drinkBehaviour.handAnim).gameObject.SetActive(false);
      drinkBehaviour.bottleHand.gameObject.SetActive(false);
      if (drinkBehaviour.canCauseBlindness && (double) Random.Range(0.0f, 1f) >= 0.800000011920929)
        drinkBehaviour.blindness.SendEvent("METHANOL");
      ((Behaviour) drinkBehaviour.playerFunctions).enabled = true;
      yield return (object) null;
    }
  }
}
