let allQuestions = [
    {
        "category": "Easy",
        "question": "How much food do each singaporeans waste everyday?",
        "options": [
            "1.4 billion kg",
            "2 bowls of rice",
            "10kg of rice",
            "744 million kg"
        ],
        "correctIndex": 1
    },
    {
        "category": "Easy",
        "question": "What are the 3 Rs of sustainbility?",
        "options": [
            "reduce, rebuild, regain",
            "reuse, reduce, regain",
            "reduce, reuse, recycle",
            "rebuild, recycle, rebuild"
        ],
        "correctIndex": 2
    },
    {
        "category": "Easy",
        "question": "how long does it take for honey to expire?",
        "options": [
            "6 months",
            "1 year",
            "10 years",
            "never"
        ],
        "correctIndex": 3
    },
    {
        "category": "Medium",
        "question": "what does NEA stand for?",
        "options": [
            " National Environment Agency ",
            "North East Association",
            "Natural Enironment Adaptation ",
            "Nature Earth Association"
        ],
        "correctIndex": 0
    }
]

const chooseRandom = (arr, num = 1) => {
    const res = [];
    for (let i = 0; i < num;) {
        const random = Math.floor(Math.random() * arr.length);
        if (res.indexOf(arr[random]) !== -1) {
            continue;
        };
        res.push(arr[random]);
        i++;
    };
    return res;
};

let questions = chooseRandom(allQuestions, 2);