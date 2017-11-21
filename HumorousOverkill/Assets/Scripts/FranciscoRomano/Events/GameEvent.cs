public enum GameEvent
{
    _NULL_,
    /**************************************************************************************************/
    /** TYPES *****************************************************************************************/

        CLASS_TYPE_ENEMY_MANAGER,
        CLASS_TYPE_ENEMY_SPAWNER,

    /**************************************************************************************************/
    /** STATES ****************************************************************************************/
    
        STATE_MENU        = 0xA00,
        STATE_PAUSE       = 0xA01,
        STATE_START       = 0xA02,
        STATE_RESTART     = 0xA03,
        STATE_CONTINUE    = 0xA04,
        STATE_DIFFICULTY  = 0xA05,
        STATE_WIN_SCREEN  = 0xA06,
        STATE_LOSE_SCREEN = 0xA07,

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

    DIFFICULTY_EASY = 0xD01,
    DIFFICULTY_MEDI = 0xD02,
    DIFFICULTY_HARD = 0xD03,
    DIFFICULTY_NM   = 0xD04,
}