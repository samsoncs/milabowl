export interface BombGameWeekState {
    gameWeek: number;
    bombState: string;
    bombHolder: BombManager;
    bombThrower?: BombManager | null;
}

export interface BombManager {
    fantasyManagerId: number;
    managerName: string;
    userName: string;
}
