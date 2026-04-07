using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* feedback i zmiany
    generalnie w takiej obiektówce trzeba unikać "god objectów" 
    uzycie #region no nie rozwiazuje problemu tylko go ukrywa
    Najwazniejsza zasada w programowaniu imo do pisania dobergo kodu 
    to single resposibility priciple - klasa jest odpowiedzialna za jedną rzecz
    GameLogicManager generalnie powinien być odpowiedzialny tylko za obsługe petli gry
    a jego delegaci - klasy pomocnicze, wykonywac okreslona logike, nie mniej szczerze imo trzeba
    pchać kod do przodu i robic refactor przy okazji
    rzeczy które myśle mozna by miec na uwadze do poprawy przy okazji:
    -rename enemie na enemy xd
    -rozbicie tej klasy na mniejsze TurnManager, CombatManager i TargetRegistry (rejestr ITargetable) dla obu stron zamiast list publicznych
    -mam wrazenie ze mozna spokojnie ujednolicic zachowanie naszych jednostek i jednostek przeciwnika wlasciwie logika jest identyczna w tychze klasach wystarczy sparametryzowac to jakims enumem Team { Player, Enemy }
    -nie znam sie na tyle na programowaniu pod unity ale imo taki tight coupling singeltonow smierdzi - imo do dema mozna zostawic
    
    -widze tez duzo pollingu (IsFight IsAlive itp) - to zła praktyka bo prowadzi do kodu spaghetti z ifologią co widać troszke w tym codebasie wlasnie przez to
    spoko praktyką jest wyciąganie tych informacji do listy,
    co mam na myśli to mamy powiedzmy klase Człowiek i potrzebujemy na potrzeby kodu wiedzieć czy ten człowiek ma włosy. 
    zamiast trzymać tą informacje bezpośrednio w Człowieku to wyciągamy ja do klasy która zarządza Człowiekami i mamy list<Człowiek> hairPersons = { malikMontana itp } i list człowiek hairlessPersons = { pudzian }
    generalnie myśle, ze takie myslenie w programowaniu bardzo pomaga - Odwaracnie zaleznosci (dependency inversion) to mega dobra praktyka 
    czasami wydaje nam sie ze abstrakcyjnie to budynki powinny generowac jednostki co nie, ale z perspektywy architektury programu lepszym byłoby zeby jakas instancja zarzadzajaca budynkami
    byla rowniez odpowiedzialna za te jednostki no bo budynek moze zniknac, a jednostki ktore naprodukował nadal zyja sobie na mapie i cos musi miec do nich referencje
     
     tldr; mieć na uwadze te uwagi przy dodawaniu ficzerow, generalnie zasada jest taka ze jak
     implementacja czegos powoduje potrzeby modyfikacji duzej ilosci kodu (generalnie kod ktory trzeba modyfikowac jest zly //nie mylić z rozszerzalnym kodem) ale tez modyfikacji duzej ilosci innych miejsc w inncyh klasach
     to trzeba do mnie napisac i wymyslimy jak zmienic architekture 
 */
public class GameLogicManager : MonoBehaviourSingleton<GameLogicManager>
{
    public Action OnEndTurn;
    public Action OnEndFight;
    public Action OnAllUnitsDead;

    public List<Transform> SpawnPoints;

    public List<EnemieUnitsManager> EnemieUnitsManagers = new();
    public List<UnitsManager> PlayerUnitsManagers = new();

    public List<ITargetable> EnemieTargets = new();
    public List<ITargetable> PlayerTargets = new();

    public bool IsFight = false;


    [SerializeField] private int _turn = 0;
    private int _moneyAmount = 3;

    private DeckManager _deckManager => DeckManager.Instance;
    private EnemieAttackProjection _enemieAttackProjection => EnemieAttackProjection.Instance;
    //temp solution
    private void Update()
    {
        if (IsFight && PlayerUnitsManagers.Count == 0 && EnemieUnitsManagers.Count == 0 ) 
        {
            OnDeathOfAllUnits();
        }
    }

    #region Units
    //public void SpawnEnemieUnits()
    //{
    //    foreach(var enemieUnitsManager in EnemieUnitsManagers)
    //    {
    //        enemieUnitsManager.Initialize(enemieUnitsManager.UnitData,
    //            SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count - 1)].position, false);
    //    }
    //}
    private void UnifyUnits()
    {
        foreach (var enemieUnitsManager in EnemieUnitsManagers)
        {
            EnemieTargets.AddRange(enemieUnitsManager.EnemieUnits);
        }
        foreach (var playerUnitsManager in PlayerUnitsManagers)
        {
            PlayerTargets.AddRange(playerUnitsManager.Units);
        }
    }
    private void DestroyAllUnits()
    {
        PlayerTargets.RemoveAll(x => x == null);
        
        foreach(UnitHandler playerUnits in PlayerTargets)
        {
            playerUnits.DestroyUnit();
        }
        
        EnemieTargets.RemoveAll(x => x == null);
        
        foreach (UnitHandler enemieUnits in EnemieTargets)
        {
            //TODO: Deal damage to player base
            if (enemieUnits == null) continue;

            enemieUnits.DestroyUnit();
        }
    }
    #endregion
    #region TurnManagement
    public void OnFightEnd(bool hasPlayerLost)
    {
        if (!IsFight)
        {
            IsFight = true;
            //DestroyAllUnits();
            //ClearLists();
            OnEndFight?.Invoke();
            //NextTurnSetup();
        }
    }
    public void OnDeathOfAllUnits()
    {
        IsFight = false;
        OnAllUnitsDead?.Invoke();
        //DestroyAllUnits();
        ClearLists();
        OnEndFight?.Invoke();
        NextTurnSetup();
    }
    public void EndTurnCheck()
    {
        if(_deckManager.IsHandEmpty())
            EndTurn();
    }
    public void EndTurn()
    {
        OnEndTurn?.Invoke();
        _deckManager.DiscardAllCardsInHand();
        IsFight = true;
    }
    public void FirstTurn()
    {
        if (_turn == 0)
        {
            _turn++;
            _deckManager.OnFirstTurn(_moneyAmount);
            //TempEnemieScaling();
            //EnemieAttackProjectionSetup();
        }
        else
            NextTurnSetup();

    }
    private void NextTurnSetup()
    {
        _turn++;
        //TempEnemieScaling();
        _deckManager.NextTurn(_moneyAmount);
        //EnemieAttackProjectionSetup();
    }
    //private void TempEnemieScaling()
    //{
    //    foreach (var enemieUnitsManager in EnemieUnitsManagers)
    //    {
    //        float scaleFactor = 1.2f;
    //        enemieUnitsManager.UnitData.UnitAmount =
    //             Mathf.RoundToInt
    //             (enemieUnitsManager.UnitData.UnitAmount * scaleFactor);
    //    }
    //}
    //private void EnemieAttackProjectionSetup()
    //{
    //    _enemieAttackProjection.ClearAttackProjections();
    //    foreach (var enemieUnitsManager in EnemieUnitsManagers)
    //    {
    //        _enemieAttackProjection.SetAttackProjection(enemieUnitsManager.UnitData.UnitSprite,
    //            enemieUnitsManager.UnitData.UnitAmount);
    //    }
    //}
    public void EnemieDefeated()
    {
        Debug.Log("Enemie Defeated");
    }
    #endregion
    #region Utilities
    private void ClearLists()
    {
        EnemieTargets.Clear();
        PlayerTargets.Clear();
        PlayerUnitsManagers.Clear();
        EnemieUnitsManagers.Clear();
    }
    #endregion
}
