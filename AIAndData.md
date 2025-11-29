# Artificial Intelligence and Big Data

## AI Introduction

techniques:

- machine learning
- deep learning
- neural networks
- natural language processing

Generative AI

- Language model: encapsulate semantic relationships between language elements. large language models vs. SLM

Computer vision

- Image classification
- Object detection: OCR
- Image segmentation

Speech

- Speech recognition
- Speech synthesis: text-to-voice

Natural language processing

- not necessarily using GenAI
- entity extraction
- Text classification
- Sentiment analysis

Products:

- Udio: From composing music based on a couple of quick prompts
- Obsidian: taking notes and organizing your thoughts
- Tray.ai: automating and connecting services
- Cursor: dedicated programs
- M365 Copilot: sensitive internal corp information

<https://learningpath.microsoft.com/8523/6de4a674-2336-4ebb-8998-faf7eacb1d23>

Large Language Models: GPT-4, ChatGPT.

AI model architecture: Transformer.

Training components:

- supervised fine-tuning: labeler give output
- Reinforcement learning from human feedback: labeler rate the output, go to reward model
- Use proximal policy optimization algorithm

### Prompt

Prompt with 4 elements

1. Goal
2. Context
3. Source
4. Expectations

### AI ideas

- Show the manual ops
- Show the challenges
- AI options and comparation
  - illustration
  - not able to follow
- the option we choose and a demo
- future

9/4 - 10/24,

### Skillup AI

<https://skilluplabforhackathon.azurewebsites.net/speedskills/researcher/performance>

- Researcher?

### RAG

RAG: Retrieval-Augmented Generation

- Chunking
- embeddings
- Store to FAISS, Chroma, Pinecone etc.

### ChatGPT Learn AI suggestion

#### What to learn (order matters)

1. Programming + tooling you’ll actually use: Python 3.11, NumPy, Pandas, PyTorch, Jupyter/VS Code, Git.
1. Core ML: supervised learning, train/val/test, bias/variance, regularization.
1. Deep Learning: backprop, CNNs, RNNs/Transformers, optimization tricks.
1. NLP & LLMs: tokenization, attention, pretraining vs finetuning, LoRA, RAG.
1. MLOps & Systems: experiment tracking, data/versioning, deployment, inference/serving, quantization.

#### A 12-week plan (5–8 hrs/week)

Weeks 1–3 — ML foundations

- Follow Andrew Ng’s Machine Learning Specialization for fundamentals. [Coursera](https://www.coursera.org/specializations/machine-learning-introduction?utm_source=chatgpt.com), [DeepLearning.ai](https://www.deeplearning.ai/courses/machine-learning-specialization/?utm_source=chatgpt.com).
- Deliverable: implement linear/logistic regression and a simple cross-validation pipeline in PyTorch.

Linear regression:

- inputs x0 - xd, weights w0 - wd, expected value y. Choose w and b to make predictions close to observed y.
- ŷ = w0​ + w1​*x1 ​+ ⋯ + wd*​xd​ = XW + b; optimize MSE (less error).

cross-validation pipeline: a repeatable procedure that splits data into multiple train/validation folds, fits your model on the train part of each fold (including all preprocessing), evaluates on the held-out part, and aggregates the scores. It estimates generalization performance and helps pick hyperparameters while avoiding leakage.

Weeks 4–6 — Deep Learning

- Do fast.ai Practical Deep Learning for Coders (free, code-first). [Practical Deep Learning](https://course.fast.ai/?utm_source=chatgpt.com), [Getting started](https://course.fast.ai/Lessons/lesson1.html?utm_source=chatgpt.com)
- Deliverable: train a small image classifier; write a short readme on overfitting fixes you used.

Weeks 7–8 — NLP basics

- Skim Stanford CS224n lectures/notes for modern NLP intuition. [Stanford University](https://web.stanford.edu/class/cs224n/?utm_source=chatgpt.com), [YouTube](https://www.youtube.com/playlist?list=PLoROMvodv4rMFqRtEuo6SGjY4XbRIVRd4)
- Deliverable: train a tiny seq model (or use a pretrained one) for text classification.

Weeks 9–10 — LLMs in practice

- Work through the Hugging Face LLM course (Transformers/Datasets/Accelerate).
- Hugging Face
- Hugging Face

Deliverable: finetune a small model with LoRA on a curated dataset; evaluate with a held-out set.

Weeks 11–12 — Ship something

- Build a RAG app (your docs → vector DB → retriever → LLM → citations).

- Stretch: add a reranker and eval harness (faithfulness, groundedness).

#### Fast 10-day jumpstart (if you want momentum now)

Day 1–2: Set up env (Python, PyTorch CUDA; Jupyter; Hugging Face CLI).
Day 3–4: Train a baseline classifier on a tabular dataset.
Day 5–6: Finetune a small LLM with LoRA on a tiny dataset; compare to zero-shot.
Day 7–8: Build minimal RAG (Chroma/FAISS, top-k=8, cite sources).
Day 9–10: Add evals + a single-file API (FastAPI) and run basic load tests.

#### Projects that teach the right muscles

Doc QA (RAG): index your own notes/docs and answer questions with citations.

Build-log anomaly detector: classify failure types from logs; surface top tokens/spans.

Code-search: embed code/comments; semantic search + keyword fallback.

Latency-aware LLM serving: compare FP16 vs INT8 vs 4-bit; graph throughput vs latency.

#### Reference tracks (pick what fits your style)

Vision track: Stanford CS231n (notes + assignments).
CS231n
+1
CS231n

NLP/LLMs track: Stanford CS224n + Hugging Face LLM course.
Stanford University
YouTube
Hugging Face

Practical, code-first: fast.ai.
Practical Deep Learning for Coders

#### Tooling (minimum viable stack)

Python 3.11, PyTorch (CUDA build), VS Code, Jupyter.

Libraries: PyTorch, scikit-learn, transformers/datasets/accelerate, faiss-cpu or Chroma, FastAPI.

Optional for serving: vLLM or TGI, ONNX/TensorRT for speed, MLflow or Weights & Biases for tracking.

#### How to study (so it sticks)

Tight loop: watch/skim → implement from scratch (tiny version) → use a library → write 5-10 bullet “learnings.”

Benchmark yourself: keep a simple leaderboard (accuracy/F1/latency/cost).

Portfolio first: every lesson ends with a commit, a README, and a demo (Colab/Spaces/local).

Evaluate rigorously: build small checklists for data leakage, eval splits, prompt safety.

If you want, tell me your available time each week and the kind of project you’re excited to ship first—I’ll tailor this plan (and give you a focused resource list + step-by-step tasks for the next 14 days).

## Tools

### ChatGPT

<https://platform.openai.com/docs/quickstart>

### Github Copilot

<https://github.com/features/copilot>

- /tests Generate unit tests for this function. Validate both success and failure, and include edge cases.
- Improve the variable names in this function. Can use copilot edits

<https://docs.github.com/en/copilot/quickstart?tool=vscode>

- Ask Copilot for help using the command line
- knowledge bases: Create and manage collections of documentation
- Code completion: next edit suggestions
- Copilot Edits: across multiple files directly from a single Copilot Chat prompt
  - Edit mode vs. agent mode

<https://docs.github.com/en/copilot/using-github-copilot/best-practices-for-using-github-copilot>

- Code completions
- built-in keywords
- tell Copilot Chat that it is a Senior C++ Developer who cares greatly about code quality, readability, and efficiency, then ask it to review your code
- Break down complex tasks.
- Be specific about your requirements.
- Provide examples of things like input data, outputs, and implementations.
- Follow good coding practices.
- Review suggestion.
- open relevant files and close irrelevant files.

<https://code.visualstudio.com/blogs/2025/02/24/introducing-copilot-agent-mode>

- `.github/copilot-instructions.md` file to let AI understand about the project

<https://github.com/modelcontextprotocol/servers>

<https://modelcontextprotocol.io/introduction>

- MCP hosts: IDEs, programs
- MCP clients: protocol clients maintaining connections
- MCP servers: services work with MCP like git
- Local resources/internet services

<https://github.com/modelcontextprotocol/python-sdk?tab=readme-ov-file>

<https://docs.astral.sh/uv/>

- Agent mode in VS.

Github Copilot CLI: <https://github.com/features/copilot/cli/>

MCP:

```
import subprocess

def build_project(project_path):
    command = ["dotnet", "build", project_path]
    result = subprocess.run(command, capture_output=True, text=True)
    print(result.stdout)
    if result.returncode != 0:
        print("Build failed:", result.stderr)

if __name__ == "__main__":
    project_path = input("Enter C# project path: ")
    build_project(project_path)
```

TODO: How to build an agent infra?

```
I am a C# developer trying to leverage AI. How to build an infra to let an agent develop code, an agent build project and an agent run the test, then let the agent fix the test failures?
```

Orchestration Models

- Orchestrator: A controller agent or framework that assigns tasks and manages flow.
- Agents: Specialized units with clear roles (e.g., planner, executor, evaluator).
- Communication Protocols: Standards like A2A (Agent-to-Agent) and MCP (Model Context Protocol) enable agents to share context and collaborate across systems.
- Shared Memory Layer: Maintains state across agents without overwriting each other’s work.

Tools:

- Semantic Kernel: framework
- Azure AI Foundry: service. Provides runtime and SDK for developing prod grate agent. Supports connected agents, long running multi-agent workflows.
- Copilot Studio: low code for building agents: Declarative agents (run on M365) vs. Custom engine agents. Has ADO integration.
- Microsoft Agent Framework: Provide orchestration patterns <https://learn.microsoft.com/en-us/training/modules/orchestrate-semantic-kernel-multi-agent-solution/>
- M365 Copilot Chat: personal productivity. Not same as M365 Copilot
- M365 Copilot: enterprise productivity, access to all Office data

Best Practices

- Define Clear Roles: Each agent should have a narrow, well-defined responsibility.
- Use Context Engineering: Persist relevant state across agents to avoid coordination failures.
- Monitor & Iterate: Track task states separately from conversation history for easier debugging.
- Start Small: Begin with two agents and scale gradually, adding orchestration logic as complexity grows.
- Ensure Governance: Apply compliance and audit standards consistently across agents.

### Cline

<https://docs.cline.bot/getting-started/for-new-coders>

- `Cline: Open In New Tab`
- `Hello Cline! I need help setting up my Windows PC for software development. Could you please help me install the essential development tools like Node.js, Git, and any other core utilities that are commonly needed for coding? I'd like you to guide me through the process step-by-step.`
- token: 3/4 of a word in English

<https://docs.cline.bot/improving-your-prompting-skills/prompting>

- `.clinerules` file

<https://docs.cline.bot/improving-your-prompting-skills/cline-memory-bank>

- basically a rule file with more structured docs

Can let it create a MCP server.

- Define core MCP server functionality
- Create basic server structure
- Implement protocol handling
- Add context management

FastAPI is a modern, high-performance web framework for building APIs with Python 3.7+ based on standard Python type hints.

Any AI tools for Outlook? Copilot is very stupid.

Use Microsoft Graph API + Python/OpenAI/GPT to extract and classify emails.

AI for writting doc

AI genetate build target

CLine rules: create a file.

When using CLine (or any LLM-based tool like it) with a multi-step prompt, it may miss steps due to several common reasons:

Token Limitations or Prompt Truncation: If the prompt is too long or complex, parts of it may be ignored or truncated internally. This can lead to steps being skipped, especially near the end of the prompt.

Lack of State Tracking: CLine doesn’t inherently track progress or maintain a checklist of what it has completed. If multiple instructions are given without enforcing sequential execution, it may selectively execute or ignore some.

Ambiguity or Overload: Prompts that include too many actions or vague instructions (e.g., "migrate this project to SDK style") can lead the model to interpret the intent broadly and miss specific tasks. The model might consider some steps implied or unnecessary based on its interpretation.

Execution Environment Side Effects: Sometimes the tool may appear to skip a step because the command silently failed or had no effect in the actual environment. The model might proceed under the assumption that it succeeded.

Heuristic Prioritization: LLMs sometimes prioritize what they believe to be the "core" or "most relevant" actions based on pattern recognition and training. This can cause lower-priority or less common steps to be dropped.

How to Mitigate
Break large prompts into smaller, focused ones, each handling 1–3 steps.

Include explicit checklist-style instructions: “Step 1: … Step 2: … Step 3: …”

Ask CLine to echo or log each step it performs (echo "Starting step 1...") to detect omissions.

Add verification checks at the end (e.g., test builds) to confirm the steps were executed.

### M365 Copilot

Researcher agent: use reasoning models. Better to do data intense investigation. Takes very long.

Build your own agent

`/` can attach files and show commands.

Copilot memory: if prefer something, can say it in a prompty. THen it will say the memory is updated. See in Setting: personalization > saved memories

### Stable diffusion

Torch cannot be installed. Maybe related to python version?

```log
Python 3.13.5 (tags/v3.13.5:6cb20a2, Jun 11 2025, 16:15:46) [MSC v.1943 64 bit (AMD64)]
Version: v1.10.1
Commit hash: 82a973c04367123ae98bd9abdf80d9eda9b910e2
Installing torch and torchvision
Looking in indexes: https://pypi.org/simple, https://download.pytorch.org/whl/cu121
ERROR: Could not find a version that satisfies the requirement torch==2.1.2 (from versions: 2.6.0, 2.7.0, 2.7.1, 2.8.0)
ERROR: No matching distribution found for torch==2.1.2
Traceback (most recent call last):
  File "D:\Github\stable-diffusion-webui\launch.py", line 48, in <module>
    main()
    ~~~~^^
  File "D:\Github\stable-diffusion-webui\launch.py", line 39, in main
    prepare_environment()
    ~~~~~~~~~~~~~~~~~~~^^
  File "D:\Github\stable-diffusion-webui\modules\launch_utils.py", line 381, in prepare_environment
    run(f'"{python}" -m {torch_command}', "Installing torch and torchvision", "Couldn't install torch", live=True)
    ~~~^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  File "D:\Github\stable-diffusion-webui\modules\launch_utils.py", line 116, in run
    raise RuntimeError("\n".join(error_bits))
RuntimeError: Couldn't install torch.
Command: "D:\Github\stable-diffusion-webui\venv\Scripts\python.exe" -m pip install torch==2.1.2 torchvision==0.16.2 --extra-index-url https://download.pytorch.org/whl/cu121
Error code: 1
Press any key to continue . . .
```

Need to follow <https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Install-and-Run-on-NVidia-GPUs>

## Agent

using just one agent for a extensive problem will be infeasible. Communication will overload its context producing unreliable/inconsistent results.

Can build a quiz agent to learn Python differences in M365

Prompting engineering has been shifted to MCP/Agent-to-Agent/Agentic system.

Agent has access to other services.

- Pick your host: GitHub Copilot Agent Mode in VS Code, Copilot Studio, or Azure AI Agent Service.
- List/enable MCP servers in the host (e.g., mcp.json in VS Code; verify Graph/Kusto/custom servers show up).
- Publish your repetitive steps as MCP tools (wrap scripts/APIs for build, test, PR, telemetry).
- Author prompts/instructions (task recipes) that call the right tools, with guardrails (required checks, timeouts, dry‑run mode).
- Secure and observe: Use Entra app registrations/permission scopes; monitor tool usage and errors

### MCP

<https://modelcontextprotocol.io/docs/getting-started/intro>

- MCP is an open protocol that standardizes how applications provide context to LLMs.​
- Agent mode can actually use MCP

server:

- Resources, Tools: LLM functions, Prompts
- STDIO-based servers vs. HTTP-based servers
- uses the [uv](https://github.com/astral-sh/uv/) tool for Python. It is new tool developed for Python use AI.

### Agent2Agent (A2A) Protocol

<https://github.com/google/A2A>

### Agent Id

<https://techcommunity.microsoft.com/blog/microsoft-entra-blog/announcing-microsoft-entra-agent-id-secure-and-manage-your-ai-agents/3827392>

## Models

### Concept

A model is a large mathematical function trained to understand and generate data.

- LLaMA and GPT are 2 models
- A giant neural network with billions of parameters, that learns probabilities of sequences of words/tokens
- training data saved in .bin, .safetensors, or .gguf
- The blueprint: how many layers, attention heads, embedding size, etc.

LLM vs. SLM vs. Nano

### GPT 5: Thinking model

- Fell like it is less smart than 4o. Ask it to rephrase. It becomes very short.
- But if open thinking mode, it is much better
- Can switch back to 4o
- How large is the context?

when going through a big list, it can lie. Thinking mode seems much better.

Claude Opus is better for large scale, Sonnet is for shorter.

### Model difference

Cloude 4 seems the best.

### AI Foundary

<https://ai.azure.com/>

- GPT are models, Translate are services.

### Azure Machine Learning

Use Azure Machine Learning

`When using identity-based authentication, "Storage Blob Data Contributor" and "Storage File Privileged Contributor" roles must be granted to individual users that need access on the storage account.`

<https://learn.microsoft.com/en-us/azure/machine-learning/overview-what-is-azure-machine-learning?view=azureml-api-2>

<https://ml.azure.com/fileexplorerAzNB?wsid=/subscriptions/7c8fdcf0-3edf-4ff4-bacb-ebab965e3d92/resourcegroups/ML-Translate/providers/Microsoft.MachineLearningServices/workspaces/ML-Translate&tid=c74a24d8-2986-4739-aec1-36b4c9934ed3&activeFilePath=Samples/SDK%20v2/tutorials/get-started-notebooks/quickstart.ipynb&notebookPivot=1>

### Local Training

NVIDIA CUDA

- A parallel computing platform for using NVIDIA GPUs.
- Used with deep learning frameworks like PyTorch or TensorFlow.

## Use case

Use LLaMA 3 8B for translate. Hugging Face + bitsandbytes (GPU + CPU hybrid).

```bash
pip install torch transformers accelerate bitsandbytes
pip install huggingface_hub
python -m venv llama3env
.\llama3env\Scripts\activate
pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu128
```

On Windows, bitsandbytes might not work natively.

Need request access to the repo first, then run `huggingface-cli login`

Can check if CUDA is enabled

```python
import torch
print(torch.cuda.is_available())
```

```python
from transformers import AutoTokenizer, AutoModelForCausalLM, pipeline, BitsAndBytesConfig

model_id = "meta-llama/Meta-Llama-3-8B-Instruct"

bnb_config = BitsAndBytesConfig(
    load_in_4bit=True, # quantized loading
    bnb_4bit_quant_type="fp4",  # for GPU
    bnb_4bit_compute_dtype="float16",
    bnb_4bit_use_double_quant=True
)

tokenizer = AutoTokenizer.from_pretrained(model_id, use_auth_token=True)

model = AutoModelForCausalLM.from_pretrained(
    model_id,
    quantization_config=bnb_config,
    device_map="auto",        # offload to available devices
    use_auth_token=True
)

pipe = pipeline("text-generation", model=model, tokenizer=tokenizer)
response = pipe("Translate Japanese to Chinese: 日本では、春になると桜が咲きます。", max_new_tokens=100)
print(response[0]["generated_text"])
```

<https://pytorch.org/get-started/locally/>

`max_new_tokens=1000` can make a big difference.

Difference between deepseek-ai/DeepSeek-R1-Distill-Llama-8B and deepseek-ai/DeepSeek-R1-Distill-Qwen-7B

- Architecture between Meta’s LLaMA 3 or Alibaba’s Qwen (stronger in Chinese)

```python
messages = [
    {"role": "user", "content": "请把这句日文翻译成中文:\n日本では、春になると桜が咲きます。"}
]

input_ids = tokenizer.apply_chat_template(messages, return_tensors="pt").to("cuda")
output_ids = model.generate(input_ids, max_new_tokens=5000)
response = tokenizer.decode(output_ids[0], skip_special_tokens=True)
```

Cache folder: `C:\Users\<user>\.cache\huggingface\hub\`

Trying model `facebook/nllb-200-distilled-600M`

Got error: `ValueError: Unrecognized configuration class <class 'transformers.models.m2m_100.configuration_m2m_100.M2M100Config'> for this kind of AutoModel: AutoModelForCausalLM.`

To use it, `pip install sentencepiece`, `python -m pip install --upgrade pip setuptools wheel`

The SentencePiece need to be built.

`pip install huggingface_hub[hf_xet]` or `pip install hf_xet`

Need to install MSVC to compile C++.

### Translate

- <https://www.youtube.com/watch?v=B3SZCV0IwHU>
- <https://docs.2sj.ai/draw/nono>
- <https://arxiv.org/abs/1706.03762>

### Solve game challenge

xxx
?xx
?xx
x?x
xxx
xD?
xxx
OEx

Split
Next
Portal*2

Here is a puzzle. Below is a board. s means start. x is a normal cell. ? needs to be filled with a special cell in each step, but it can be replaced in the next step. D means a door. O means the opener of the door. E is exit.
sxx
?xx
?xx
x?x
xxx
xD?
xxx
OEx

Special cells are

1. split: a ball split into 2, the original ball goes down. and the new ball goes right (or disappear if it is the last column).
2. next: just go forward
3. portal * 2: go from one portal to another.

A ball start from s, every step it moves 1 cell down if the cell is a normal cell, or trigger a special cell. If it moves to the door but the door is closed, and if the ball is the original ball, then it start over. Otherwise it disappear.

### Dotnet Core Migration

<https://dotnet.microsoft.com/en-us/platform/upgrade>

## Big Data

### Map Reduce

Map:

- Key: file location
- Value: file content

Reduce:

- Key, value: OutputCollector returned by Map, which values are already sorted, and only for this key.

```java
void Map::map(Key key, Value value, OutputCollector<ReduceKey, ReduceValue> output);

void Reduce::reduce(ReduceKey key, Iterator<ReduceValue> values, OutputCollector<OutputKey, OutputValue> output);
```

#### Simple example

<http://www.jiuzhang.com/solutions/word-count/>

<http://www.jiuzhang.com/solutions/inverted-index-map-reduce/>

<http://www.jiuzhang.com/solutions/anagram-map-reduce/>

#### BFS

<http://www.johnandcailin.com/blog/cailin/breadth-first-graph-search-using-iterative-map-reduce-algorithm>

```java
void map(int nodeID, string nodeStruct, OutputCollector<int, string> output) {
    Node node = parseStruct(nodeStruct); // edges|dist|status
    if (node.status == visiting) {
        for (int neiID : node.edges) {
            Node nei = {NULL, node.dist + 1, visiting};
            output.collect(neiID, nei.to_string());
        }
        node.status= visited;
    }
    output.collect(nodeID, node.to_string());
}

void reducer(int nodeID, Iterator<string> nodeStructs, OutputCollector<int, string> output) {
    Node node;
    for (string structStr : nodeStructs) {
        Node nodeStruct = parseStruct(structStr);
        if (nodeStruct.edges != NULL)
            node.edges = nodeStruct.edges;
        if (nodeStruct.dist < node.dist) // default is INT_MAX
            node.dist = nodeStruct.dist;
        if (nodeStruct.status > node.status) // visited > visiting > none
            node.status = nodeStruct.status;
    }
    output.collect(nodeID, node.to_string());
}
```

### Spark

<https://timilearning.com/posts/mit-6.824/lecture-15-spark/>

## Machine Learning

<http://open.163.com/special/opencourse/machinelearning.html>
