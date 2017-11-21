public enum GameEvent
{
    _NULL_,
    /**************************************************************************************************/
    /** TYPES *****************************************************************************************/

        CLASS_TYPE_ENEMY_MANAGER,
        CLASS_TYPE_ENEMY_SPAWNER,

    /**************************************************************************************************/
    /** STATES ****************************************************************************************/
    
        STATE_MENU,
        STATE_PAUSE,
        STATE_START,
        STATE_RESTART,
        STATE_CONTINUE,

    /**************************************************************************************************/
    /** ENEMY SPAWNER STATES **************************************************************************/

        ENEMY_DIED,
        //ENEMY_SPAWN,
        ENEMY_DAMAGED,


        ENEMY_SPAWNER_NEXT,
        ENEMY_SPAWNER_BEGIN,
        ENEMY_SPAWNER_CREATE,
        ENEMY_SPAWNER_FINISH,
        ENEMY_SPAWNER_REMOVE,

    /**************************************************************************************************/
    // UI Enum
    UI_HEALTH,
    UI_AMMO_MAX,
    UI_AMMO_CUR,
    /**************************************************************************************************/
    // Player Enum
    PICKUP_HEALTH,
    PICKUP_RIFLEAMMO,
    PICKUP_SHOTGUNAMMO,
    PLAYER_DAMAGE,
    /**************************************************************************************************/

    DIFFICULTY_EASY,
    DIFFICULTY_MEDI,
    DIFFICULTY_HARD,
    DIFFICULTY_NM,
}