import { ResponsiveBoxPlot } from '@nivo/boxplot';
import colors from 'tailwindcss/colors';
import type { GameWeekResult } from '../../game_state/gameState';

interface Props {
    overallScore: GameWeekResult[];
}

const RuleBoxPlot: React.FC<Props> = ({ overallScore }) => {
    const data = overallScore.flatMap((o) => 
        o.rules.map(r => ({group: r.ruleShortName, value: r.points}))
    );
    return (
        <ResponsiveBoxPlot
            data={data.sort((a, b) => b.value - a.value)}
            borderWidth={4}
            whiskerWidth={4}
            borderRadius={2}
            whiskerEndSize={0.6}
            inactiveOpacity={0.25}
            margin={{ top: 120, right: 250, bottom: 200, left: 60 }}
            axisLeft={{
                tickSize: 5,
                tickPadding: 5,
                tickRotation: 0,
                legend: 'value',
                legendPosition: 'middle',
                legendOffset: -40,
                truncateTickAt: 0,
            }}
            borderColor={{
                from: 'color',
                modifiers: [['darker', 0.5]],
            }}
            medianColor={{
                from: 'color',
                modifiers: [['darker', 0.5]],
            }}
            whiskerColor={{
                from: 'color',
                modifiers: [['darker', 0.5]],
            }}
            colors={[colors.indigo[300]]}
            theme={{
                text: {
                    fill: '#ede9fe',
                },
                translation: {},
                tooltip: {
                    container: {
                        color: 'black',
                    },
                },
            }}
        />
    );
};

export default RuleBoxPlot;
