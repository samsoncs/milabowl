export type BombHistoryState = {
    bombHistoryByGameWeek: Record<number, BombEvent[]>;
    currentRoundEmojis: BombEmojiAndManager[];
    currentState: CurrentBombState;
}

export type BombEvent = {
    description: string;
    emoji: string;
    severity: string;
}

export type BombEmojiAndManager = {
    fantasyManagerId: number;
    userId: number;
    emoji: string;
}

export type CurrentBombState = {
    weeksSinceLastExplosion: number;
    bombTier: string;
    bombEmoji: string;
}