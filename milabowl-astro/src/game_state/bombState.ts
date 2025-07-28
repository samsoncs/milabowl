export interface BombGameWeekState {
    gameWeek: number;
    bombState: string;
    bombHolder: BombManager;
    bombThrower?: BombManager | null;
    bombTier: string;
    weeksSinceLastExplosion: number;
    collateralTargets: BombManager[];
    collateralTargetPlayerName?: string | null;
}

export interface BombManager {
    fantasyManagerId: number;
    managerName: string;
    userName: string;
}